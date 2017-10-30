/*jshint esversion: 6*/
import { Sprite, ResourceManager as RM } from 'athenajs';

class DeathExplosion extends Sprite {
    constructor(options) {
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
                        offsetX: 396,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 462,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 528,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 594,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 660,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 726,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 792,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 858,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 924,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 990,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 1056,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 1122,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 1188,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 1254,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 1320,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 1386,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    },
                    {
                        offsetX: 1452,
                        offsetY: 298,
                        width: 64,
                        height: 64,
                        plane: 0
                    }],
                    loop: 0
                }
            }
        });
        // options = options || {};

        // options.x = typeof options.x !== 'undefined' ? options.x : 600;
        // options.y = typeof options.y !== 'undefined' ? options.y : 300;
    }
    reset() {
        super.reset();

        this.currentMovement = '';
        this.setAnimation('mainLoop');

        this.running = true;
    }
}

RM.registerScript('DeathExplosion', DeathExplosion);

export default DeathExplosion;

