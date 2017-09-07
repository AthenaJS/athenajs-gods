/*jshint esversion: 6*/
import { BitmapText, ResourceManager as RM } from 'athenajs';

class BitmapFont extends BitmapText {
    constructor(type, options) {
        super(type, Object.assign(options, {
            offsetX: 34,
            startY: 36,
            charWidth: 18,
            charHeight: 18,
            imageId: 'font'
        }));
    }
}

RM.registerScript('BitmapFont', BitmapFont);

export default BitmapFont;