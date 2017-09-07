/*jshint esversion: 6*/
import { Sprite, ResourceManager as RM } from 'athenajs';

class FlyingEnemy1 extends Sprite {
    constructor(options = {}) {
        // options = options || {};

        super('flying-enemy1', {
            imageId: 'enemies',
            x: options.x,
            y: options.y,
            collideGroup: 1,
            canCollide: true,
            canCollideFriendBullet: true,
            pool: options.pool,
            /*                        canCollide: true,*/
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
                flyRight: {
                    frameDuration: 4,
                    frames: [{
                        offsetX: 198,
                        offsetY: 364,
                        w: 64,
                        h: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 264,
                        offsetY: 364,
                        w: 64,
                        h: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 330,
                        offsetY: 364,
                        w: 64,
                        h: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    }],
                    loop: 1
                },
                flyLeft: {
                    frameDuration: 4,
                    frames: [{
                        offsetX: 132,
                        offsetY: 364,
                        w: 64,
                        h: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 66,
                        offsetY: 364,
                        w: 64,
                        h: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    },
                    {
                        offsetX: 0,
                        offsetY: 364,
                        w: 64,
                        h: 64,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 63,
                            y2: 63
                        },
                        plane: 0
                    }],
                    loop: 1
                }
            }
        });

        options.x = typeof options.x !== 'undefined' ? options.x : 600;
        options.y = typeof options.y !== 'undefined' ? options.y : 300;
    }
    reset() {
        var that = this;

        super.reset();

        this.dir = 'left';

        this.setBehavior('path', {
            nodes: [-2, 0, -2, 0, -1, 0, -1, 0, -1, 0, -3, 0, -1, 0, -1, 0, -1, 0, -3, 0, -1, 0, -2, 0, -1, 0, -3, -1, -2, 0, -2, 0, -2, -1, -2, 0, -2, -1, -3, -1, -2, -1, -3, -1, -2, -1, -2, -1, -3, -1, -3, -1, -2, -1, -4, -2, -3, -2, -3, -1, -2, -1, -3, -2, -2, -1, -4, -2, -2, -1, -3, -2, -2, -2, -4, -2, -3, -3, -2, -1, -4, -2, -2, -3, -2, -2, -4, -3, -2, -2, -2, -2, -3, -3, -2, -3, -3, -2, -2, -4, -3, -3, -2, -4, -2, -3, -3, -5, -2, -3, -4, -8, -2, -3, -2, -4, -2, -3, -3, -5, -1, -3, -3, -5, -2, -3, -3, -5, -2, -5, -3, -4, -3, -5, -2, -4, -3, -5, -1, -5, -3, -4, -2, -4, -3, -4, -2, -5, -3, -3, -6, -10, -2, -4, -3, -5, -3, -4, -3, -4, -2, -4, -3, -4, -3, -5, -2, -3, -3, -5, -2, -5, -3, -4, -2, -4, -2, -3, -3, -5, -2, -3, -2, -4, -2, -3, -2, -3, -3, -4, -3, -3, -2, -4, -3, -3, -2, -4, -3, -3, -3, -5, -2, -2, -3, -4, -5, -5, -1, -3, -2, -2, -3, -2, -2, -3, -2, -2, -3, -2, -2, -2, -2, -2, -3, -2, -2, -2, -2, -2, -2, -1, -4, -1, -2, -2, -2, -1, -3, -1, -3, -2, -4, -1, -4, -2, -8, -2, -3, -1, -4, -1, -3, 0, -4, -1, -4, 0, -6, 0, -4, 0, -4, 0, -6, 0, -4, 0, -6, 0, -4, 1, -4, 1, -5, 1, -4, 2, -3, 1, -5, 2, -3, 3, -5, 3, -3, 1, -4, 3, -3, 4, -5, 2, -4, 5, -4, 5, -2, 3, -4, 6, -3, 5, -3, 6, -5, 10, -2, 5, -1, 4, 0, 3, 0, 3, 0, 3, 0, 4, 0, 5, 0, 4, 0, 4, 2, 7, 1, 5, 2, 5, 2, 6, 1, 4, 3, 5, 2, 5, 2, 4, 7, 9, 2, 3, 4, 4, 3, 3, 3, 3, 5, 5, 5, 2, 4, 4, 6, 3, 5, 3, 6, 3, 6, 2, 5, 2, 6, 2, 6, 2, 7, 1, 6, 2, 5, 1, 6, 0, 7, 1, 5, 1, 6, 1, 5, 0, 6, 0, 5, 0, 6, 0, 10, -2, 5, -2, 4, -2, 5, -3, 4, -2, 4, -1, 3, -2, 5, -3, 4, -2, 5, -2, 2, -3, 3, -2, 3, -2, 2, -2, 4, -4, 2, -2, 3, -3, 1, -4, 3, -3, 1, -3, 2, -5, 1, -4, 3, -10, 1, -5, 2, -4, 1, -5, 0, -4, 2, -4, 0, -5, 1, -4, 0, -5, 0, -6, 0, -5, -1, -6, -1, -6, -2, -5, -2, -6, -1, -5, -1, -4, -2, -6, -3, -4, -2, -6, -3, -6, -3, -5, -3, -6, -3, -4, -4, -6, -3, -5, -5, -5, -5, -5, -10, -10, -6, -5, -6, -4, -6, -4, -6, -4, -6, -4, -6, -3, -6, -3, -6, -3, -6, -3, -4, -2, -6, -2, -4, 0, -5, -2, -4, -1, -3, -1, -2, 0, -3, -1, -3, 0, -2, -1, -3, 0, -2, -1, -4, 0, -2, -1, -2, 0, -1, 0, -1, 0],
            reverse: true,
            onVXChange: function () {
                if (that.dir === 'left') {
                    that.setAnimation('flyRight');
                    that.dir = 'right';
                } else {
                    that.setAnimation('flyLeft');
                    that.dir = 'left';
                }
            },
            onEnd: () => {
                this.movable = false;
            }
        });

        this.currentMovement = '';
        this.setAnimation('flyLeft');

        this.running = true;
    }
}

RM.registerScript('FlyingEnemy1', FlyingEnemy1);

export default FlyingEnemy1;