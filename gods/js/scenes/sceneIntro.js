import { Scene, Text, Sprite, Menu, AudioManager as AM, EventManager as EM } from 'AthenaJS';
// import Scene from 'Scene/Scene';
// import Text from 'Object/Text';
// import Sprite from 'Object/Sprite';
// import Menu from 'Object/Menu';
// import AM from 'Audio/AudioManager';
// import EM from 'Event/EventManager';

console.log(Scene, Text, Sprite, Menu, AM, EM);

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

        EM.clearEvents();

        EM.installKeyCallback('ENTER', 'up', function () {
            that.animate('Fade', {
                startValue: 1,
                endValue: 0,
                duration: 1000
            }).done(function () {
                console.log('fade ended');
                that.notify('game:gotoMenu');
            });
        });

        AM.play('restart');

        that.animate('Fade', {
            startValue: 0,
            endValue: 1,
            duration: 1000
        }).done(function () {
            console.log('fade ended');
        });

        this.addObject(this.titleScreen);

        // TODO: fadeIn
    }

    stop() {
        EM.clearEvents();
        super.stop();
    }

    debug() {}
    run() {
        //                var rotate = this.menuObject.getAngle();

        // move
        // move enemies

        // move special objects: traps, treasures, weapons,...

        // Move sprites (on Map ?!);
        // scrolling
        //                this.map.move();
        //                rotate++;
        //                this.menuObject.setAngle(rotate > 360 ? 0 : rotate);
        super.run();
    }
};

console.log('end sceneIntro');

export default new SceneIntro();