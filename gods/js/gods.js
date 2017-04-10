import sceneIntro from 'scenes/sceneIntro';
import sceneLevel1 from 'scenes/sceneLevel1';
import sceneMenu from 'scenes/sceneMenu';

import { Game, Dom } from 'athenajs';

/* Automatically append every sprite files to the Gods build */
var req = require.context("sprites/", false, /^(.*\.(js$))[^.]*$/igm);
req.keys().forEach(function(key){
    req(key);
});

/*jshint devel: true*/
class GodsClass extends Game {
    constructor(options) {
        super(options);
        // intro
        this.currentLevel = -1;

        /* debug stuff */
		this.moveMouseCb = this.moveInspector.bind(this);
    }

    onEvent(event) {
        switch (event.type) {
            case 'game:gotoMenu':
                this.setScene(sceneMenu);
                break;

            case 'game:startGame':
                this.setScene(sceneLevel1);
                break;

            case 'game:gameover':
            case 'game:exitLevel':
                // this.stopScene();
                this.setScene(sceneIntro);
                break;
        }
    }

	toggleTileInspector() {
		if (this.scene.map && this.scene.map.isDebug) {
            if (!this.tileInspector) {
                this.tileInspector = new Dom('div').css({
                    border: '1px dotted white',
                    'background-color': 'rgba(0,0,0,.7)',
                    color: 'white',
                    width: `${this.scene.map.tileWidth}px`,
                    height: `${this.scene.map.tileHeight}px`,
                    'z-index': 10,
                    position: 'absolute',
                    'pointer-events': 'none'
                }).appendTo(this.target);                
            }
			this.tileInspector.show();
			this.target.addEventListener('mousemove', this.moveMouseCb, false);
		} else {
			this.target.removeEventListener('mousemove', this.moveMouseCb);
			this.tileInspector.hide();
		}
	}

	moveInspector(event) {
		console.log(event.offsetX, event.offsetY);
        const map = this.scene.map;
        const offsetX = event.offsetX > 0 ? event.offsetX : 0;
        const offsetY = event.offsetY > 0 ? event.offsetY : 0;
        const pos = map.getTilePos(offsetX, offsetY);
        this.tileInspector.html(`${pos.x}, ${pos.y}`).css({
            left: (pos.x * map.tileWidth) + 'px',
            top: (pos.y * map.tileHeight) + 'px'
        });
	}
};

window.Gods = new GodsClass({
    debug: false,
    name: 'Gods',
    target: '.main',
    showFps: true,
    width: 1024,
    height: 768,
    sound: true
});

window.Gods.onReady(function () {
    // this.setScene(sceneIntro);
    this.setScene(sceneLevel1);

    // debug stuff
    document.body.addEventListener('keyup', (event) => {
        if (event.keyCode === 68) {
            console.log('debug');
            // scene debug
            this.scene.debug();
            this.toggleTileInspector();
        }
        if (event.keyCode === 82) {
            this.scene.stop();

            this.scene.resume();
        }

        if (event.keyCode === 80) {
            this.togglePauseGame();
        }

        if (event.keyCode === 83) {
            this.toggleSound(!that.sound);
        }

        if (event.keyCode === 72) {
            if (this.scene.hudScene) {
                var opacity = this.scene.hudScene.getOpacity();
                this.scene.hudScene.setOpacity(opacity ? 0 : 1);
            }
        }
    });
});