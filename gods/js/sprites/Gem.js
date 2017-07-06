/*jshint esversion: 6*/
import { Sprite, AudioManager as AM, ResourceManager as RM } from 'athenajs';

class Gem extends Sprite {
    constructor(options = {}) {
        super('gem', {
            imageSrc: 'objects',
            x: options.x,
            y: options.y,
            pool: options.pool,
            canCollide: true,
            collideGroup: 1,
            // animate: {
            // 	name: 'Rotate',
            // 	options: {
            // 		duration: 500,
            // 		startValue: 0,
            // 		endValue: 2*Math.PI,
            // 		easing: 'linear',
            // 		loop: true
            // 	}
            // },
            animations: {
                mainLoop: {
                    frameDuration: 4,
                    frames: [{
                        offsetX: 136,
                        offsetY: 189,
                        w: 31,
                        h: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 170,
                        offsetY: 189,
                        w: 31,
                        h: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 204,
                        offsetY: 189,
                        w: 31,
                        h: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 238,
                        offsetY: 189,
                        w: 31,
                        h: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 272,
                        offsetY: 189,
                        w: 31,
                        h: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 306,
                        offsetY: 189,
                        w: 31,
                        h: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    },
                    {
                        offsetX: 340,
                        offsetY: 189,
                        w: 31,
                        h: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    }],
                    loop: 1
                }
            }
        });

        options = options || {};

        options.x = typeof options.x !== 'undefined' ? options.x : 600;
        options.y = typeof options.y !== 'undefined' ? options.y : 300;
    }
    reset() {
        var that = this;

        super.reset();

        this.setBehavior('simplefall', {
            gravity: 0.3,
            onEnd: () => {
                this.moving = false;
            },
            onGround: function () {
                AM.play('bounce');
            }
        });

        this.currentMovement = '';
        this.setAnimation('mainLoop');

        this.running = true;
    }
}

RM.registerScript('Gem', Gem);

export default Gem;