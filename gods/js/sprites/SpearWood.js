import { Sprite, ResourceManager as RM, AudioManager as AM } from 'athenajs';
/*
import Sprite from 'Object/Sprite';
import RM from 'Resource/ResourceManager';
import AM from 'Audio/AudioManager';
*/

class SpearWood extends Sprite {
    constructor(options) {
        super('spearWood', {
            imageId: 'objects',
            x: options.x,
            y: options.y,
            pool: options.pool,
            canCollide: true,
            collideGroup: 1,
            data: options.data || {},
            animations: {
                mainLoop: {
                    frameDuration: 1,
                    frames: [{
                        offsetX: 0,
                        offsetY: 258,
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

        options = options || {};

        this.soundRef = null;
    }
    reset() {
        super.reset();

        this.currentMovement = '';
        this.setAnimation('mainLoop');

        this.addSpearSprite();

        /*                this.soundRef = AM.play('spike', true);*/
    }
    addSpearSprite() {
        this.addChild(
            new (RM.getResourceById('Spear'))({
                x: this.x,
                y: this.y - 16 /* bad: hardcoded */
            })
        );
    }
    destroy() {
        if (this.soundRef) {
            AM.stop('spike', this.soundRef);
            this.soundRef = null;
        }

        // do not forget to call the super method!
        super.destroy();
    }
};

RM.registerScript('SpearWood', SpearWood);

export default SpearWood;