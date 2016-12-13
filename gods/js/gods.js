import sceneIntro from 'scenes/sceneIntro';
import sceneLevel1 from 'scenes/sceneLevel1';
import sceneMenu from 'scenes/sceneMenu';

import { Game } from 'athenajs';

console.log('** game', Game);

/* Automatically append every sprite files to the Gods build */
var req = require.context("sprites/", false, /^(.*\.(js$))[^.]*$/igm);
req.keys().forEach(function(key){
    req(key);
});

/*jshint devel: true*/
$(function () {
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
                    this.setScene(sceneLevel1);
                    break;

                case 'game:gameover':
                case 'game:exitLevel':
                    // this.stopScene();
                    this.setScene(sceneIntro);
                    break;
            }
        }
    };

    window.Gods = new GodsClass({
        debug: false,
        name: 'Gods',
        target: $('.main'),
        fpsElt: $('.fps')[0],
        showFps: true,
        width: $('.main').width(),
        height: $('.main').height(),
        sound: true
    });

    window.Gods.onReady(function () {
        // this.setScene(sceneIntro);
        this.setScene(sceneLevel1);

        // debug stuff
        $('body').keyup( (event) => {
            if (event.keyCode === 68) {
                console.log('debug');
                // scene debug
                this.scene.debug();
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
});