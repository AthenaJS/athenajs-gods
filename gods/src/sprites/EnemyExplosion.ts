/*jshint esversion: 6*/
import { Sprite, AudioManager as AM, ResourceManager as RM } from 'athenajs';

class EnemyExplosion extends Sprite {
    constructor(options = { x: 0, y: 0, maxEnergy: 0, pool: undefined }) {
        // options = options || {};

        super('death_explosion', {
            imageId: 'enemies',
            x: options.x,
            y: options.y,
            pool: options.pool,
            canCollide: false,
            animations: {
                mainLoop: {
                    frameDuration: 3,
                    frames: [{
                        offsetX: 594,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 660,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 726,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 792,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 858,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 924,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 990,
                        offsetY: 364,
                        width: 64,
                        height: 64,
                        plane: 0
                    }],
                    loop: 0
                }
            }
        });

        options.x = typeof options.x !== 'undefined' ? options.x : 600;
        options.y = typeof options.y !== 'undefined' ? options.y : 300;
    }
    reset() {

        super.reset();

        this.currentMovement = '';
        this.setAnimation('mainLoop', function () {
            this.destroy();
        });

        this.running = true;

        AM.play('explode2');
    }
}

// RM.registerScript('EnemyExplosion', EnemyExplosion);

export default EnemyExplosion;