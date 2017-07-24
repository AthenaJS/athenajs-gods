import sceneIntro from 'scenes/sceneIntro';
import sceneLevel1 from 'scenes/sceneLevel1';
import sceneMenu from 'scenes/sceneMenu';

import { Game, Dom } from 'athenajs';

/* Automatically append every sprite files to the Gods build: since there's no way to dynamically load
   scripts when using Webpack we tell it to add every sprite to our build.
 */
var req = require.context("sprites/", false, /^(.*\.(js$))[^.]*$/igm);
req.keys().forEach(function (key) {
    req(key);
});

/*jshint devel: true*/
class GodsClass extends Game {
    constructor(options) {
        super(options);
        // intro
        this.currentLevel = -1;
    }

    onEvent(event) {
        switch (event.type) {
            case 'game:gotoMenu':
                this.setScene(sceneMenu);
                break;

            case 'game:startGame':
                debugger;
                this.setScene(sceneLevel1, true);
                break;

            case 'game:gameover':
            case 'game:exitLevel':
                // this.stopScene();
                this.setScene(sceneIntro);
                break;
        }
    }
};

window.gods = new GodsClass({
    debug: true,
    name: 'Gods',
    target: '.main',
    showFps: true,
    width: 1024,
    height: 768,
    sound: true
});

gods.setScene(sceneIntro);

// debug stuff
document.body.addEventListener('keyup', (event) => {
    if (event.keyCode === 82) {
        gods.scene.stop();

        gods.scene.resume();
    }

    if (event.keyCode === 80) {
        gods.togglePauseGame();
    }

    if (event.keyCode === 83) {
        gods.toggleSound(!gods.sound);
    }

    if (event.keyCode === 72) {
        if (gods.scene.hudScene) {
            var opacity = gods.scene.hudScene.getOpacity();
            gods.scene.hudScene.setOpacity(opacity ? 0 : 1);
        }
    }
});