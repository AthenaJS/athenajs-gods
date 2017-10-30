import { Sprite, AudioManager as AM, ResourceManager as RM } from 'athenajs';
/*
import Sprite from 'Object/Sprite';
import AM from 'Audio/AudioManager';
*/
class Switch extends Sprite {
    constructor(options = {}) {
        super('switch', {
            imageId: 'objects',
            x: options.x,
            y: options.y,
            pool: options.pool,
            objectId: options.objectId || '',
            animations: {
                unactivated: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 204,
                        offsetY: 37,
                        width: 31,
                        height: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    }],
                    loop: 0
                },
                activated: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 238,
                        offsetY: 37,
                        width: 31,
                        height: 31,
                        hitBox: {
                            x: 0,
                            y: 0,
                            x2: 31,
                            y2: 31
                        },
                        plane: 0
                    }],
                    loop: 0
                }
            }
        });

        var that = this;

        // options = options || {};

        options.x = typeof options.x !== 'undefined' ? options.x : 600;
        options.y = typeof options.y !== 'undefined' ? options.y : 300;

        // switch-specific options
        this.isActivated = options.isActivated || false;
        // /switch-specific options

        this.running = true;
    }
    toggleSwitch() {
        this.isActivated = !this.isActivated;

        this.setAnimationFromSwitch();

        AM.play('leverActivated', this.soundRef);
    }
    reset() {
        super.reset();

        this.currentMovement = '';

        this.setAnimationFromSwitch();
    }
    setAnimationFromSwitch() {
        this.setAnimation(this.isActivated ? 'activated' : 'unactivated');
    }
};

RM.registerScript('Switch', Switch);

export default Switch;