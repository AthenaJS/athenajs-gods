import { Scene, Sprite, AudioManager as AM } from 'athenajs';

class SceneIntro extends Scene {
    titleScreen: Sprite;

    constructor() {
        super({
            name: 'intro',
            resources: [
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
    }

    setup() {
        // this.setBackgroundImage('gods/img/godsIntro.jpg');

        this.titleScreen = new Sprite('intro', {
            imageId: 'intro',
            x: 0,
            y: 0,
            animations: {
                intro: {
                    speed: 3,
                    frames: [{
                        offsetX: 0,
                        offsetY: 0,
                        width: 1024,
                        height: 768,
                        hitBox: {}
                    }],
                    loop: 0
                }
            }
        });

        this.addObject(this.titleScreen);
    }
    start() {
        AM.play('restart');

        this.fadeInAndOut(1000, 2000, 1000).then(() => {
            this.notify('game:startGame');
        });
    }
};

export default SceneIntro;