/*jshint esversion: 6*/
import { Sprite, ResourceManager as RM } from 'athenajs';

class LifeMetter extends Sprite {
    constructor(options = {}) {
        // options = options || {};

        super('lifemetter', {
            imageSrc: 'objects',
            x: options.x,
            y: options.y,
            pool: options.pool,
            animations: {
                mainLoop: {
                    frameDuration: 4,
                    frames: [{
                        offsetX: 136,
                        offsetY: 227,
                        w: 48,
                        h: 48,
                        plane: 0
                    },
                    {
                        offsetX: 202,
                        offsetY: 227,
                        w: 48,
                        h: 48,
                        plane: 0
                    },
                    {
                        offsetX: 268,
                        offsetY: 227,
                        w: 48,
                        h: 48,
                        plane: 0
                    },
                    {
                        offsetX: 334,
                        offsetY: 227,
                        w: 48,
                        h: 48,
                        plane: 0
                    }],
                    loop: 1
                }
            }
        });

        options.x = typeof options.x !== 'undefined' ? options.x : 600;
        options.y = typeof options.y !== 'undefined' ? options.y : 300;

        this.maxEnergy = options.maxEnergy || 10;

        // this.addMask();
    }
    /**
     * Resets the sprite to its default (start) state: full energy => mask.height == 0
     */
    reset() {
        this.currentMovement = '';
        this.setAnimation('mainLoop');
        this.maskHeight = 0;

        this.running = true;
    }

    resetEnergy() {
        this.setEnergyMask(0);
    }

    setEnergyMask(height) {
        this.maskHeight = height;
        this.setMask({ x: 13, y: 12, w: 22, h: height }, true);
    }

    updateMetterHeight(hitPoints) {
        // TODO: this should be animated (inside lifeMetterMask ?)
        let height = this.maskHeight;

        height += 2 * hitPoints;

        if (height > 24) {
            height = 24;
        } else if (height <= 0) {
            height = 0;
        }

        this.setEnergyMask(height);

        /*                maskSprite.animate('Custom', {
                            duration: 400,
                            startValue: height,
                            endValue: height + (2 * hitPoints),
                            easing: 'linear',
                            callback: function(value) {
                                maskSprite.height = parseInt(value);
                            }
                        });*/
    }
}

RM.registerScript('LifeMetter', LifeMetter);

export default LifeMetter;