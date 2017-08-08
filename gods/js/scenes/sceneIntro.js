import { Scene, Text, Sprite, Menu, AudioManager as AM, InputManager as Input } from 'athenajs';

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
        console.log('retour super sceneIntro');
    }

    setup() {
        // this.setBackgroundImage('gods/img/godsIntro.jpg');
        // super.onLoad();
        // var that = this;

        console.log('[scene ' + this.name + '] ' + 'setup');

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

        this.addObject(this.titleScreen);
    }
    start() {
        // super.start();

        // var that = this;

        // *** Input.clearEvents();

        /*
        Input.installKeyCallback('ENTER', 'up', () => {
        });
        */

        AM.play('restart');

        this.fadeInAndOut(1000, 2000, 1000).then(() => {
            this.notify('game:startGame');
        });
        // this.fadeIn(1000, 2000).then(() => {
        //     this.fadeOut(1000).then(() => {
        //         console.log('fade ended');
        //         this.notify('game:startGame');
        //     });
        // });
    }

    // stop() {
    //     Input.clearEvents();
    //     super.stop();
    // }
};

console.log('end sceneIntro');

export default new SceneIntro();