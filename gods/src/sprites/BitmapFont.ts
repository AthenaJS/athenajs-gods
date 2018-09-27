/*jshint esversion: 6*/
import { BitmapText, BitmapTextOptions } from 'athenajs';

class BitmapFont extends BitmapText {
    constructor(type: string, options: BitmapTextOptions) {
        super(type, Object.assign(options, {
            offsetX: 34,
            startY: 36,
            charWidth: 18,
            charHeight: 18,
            imageId: 'font'
        }));
    }
}

// RM.registerScript('BitmapFont', BitmapFont);

export default BitmapFont;