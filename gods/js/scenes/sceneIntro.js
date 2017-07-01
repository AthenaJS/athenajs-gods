import { Scene, Text, Sprite, Menu, AudioManager as AM, InputManager as Input } from 'athenajs';
// import Scene from 'Scene/Scene';
// import Text from 'Object/Text';
// import Sprite from 'Object/Sprite';
// import Menu from 'Object/Menu';
// import AM from 'Audio/AudioManager';
// import Input from 'Input/InputManager';

console.log(Scene, Text, Sprite, Menu, AM, Input);

class SceneIntro extends Scene {
    constructor() {
        super({
            name: 'intro',
            resources: [
                // images
                {
                    id: 'intro',
                    type: 'image',
                    src: 'gods/img/godsIntro.jpg'
                },

                {
                    id: 'restart',
                    type: 'audio',
                    src: 'gods/audio/restart.mp3'
                }
            ]
        });
        console.log('retur super sceneIntro');
    }

    onLoad() {
        super.onLoad();
        var that = this;

        console.log('[scene ' + this.name + '] ' + 'onLoad');

        this.titleScreen = new Sprite('intro', {
            imageSrc: 'intro',
            x: 0,
            y: 0,
            animations: {
                intro: {
                    speed: 3,
                    frames: [{
                        offsetX: 0,
                        offsetY: 0,
                        w: 1024,
                        h: 768,
                        hitBox: {}
                    }],
                    loop: 0
                }
            }
        });
    }
    start() {
        super.start();

        var that = this;

        Input.clearEvents();

        /*
        Input.installKeyCallback('ENTER', 'up', () => {
        });
        */

        AM.play('restart');

        this.animate('Fade', {
            startValue: 0,
            endValue: 1,
            duration: 1000
        }).then(() => {
            setTimeout(()=> {
                this.animate('Fade', {
                    startValue: 1,
                    endValue: 0,
                    duration: 1000
                }).then(() => {
                    console.log('fade ended');
                    this.notify('game:startGame');
                });                
            }, 2000);
        });

        this.addObject(this.titleScreen);
    }

    stop() {
        Input.clearEvents();
        super.stop();
    }

    debug() {}
    run() {
        //                var rotate = this.menuObject.getAngle();
        super.run();
    }
};

console.log('end sceneIntro');

export default new SceneIntro();