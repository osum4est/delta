package com.eightbitforest.delta.utils;

import com.badlogic.gdx.graphics.Color;

public class Colors {
    public static final Color BLUE = new Color(0x66d9efff);
    public static final Color GREEN = new Color(0xa6e22eff);
    public static final Color MAGENTA = new Color(0xf92672ff);
    public static final Color ORANGE = new Color(0xfd971fff);
    public static final Color WHITE = new Color(0xf8f8f2ff);
    public static final Color BLACK = new Color(0x272822ff);

    public static final Color BACKGROUND  = BLACK;
    public static final Color WALL = WHITE;
    public static final Color EXIT = ORANGE;
    public static final Color PLAYER = BLUE;
    public static final Color FUEL = MAGENTA;

    public static Color fromHex(String hex) {
        hex = hex.replace("#", "");
        if (hex.length() == 6) {
            hex += "FF";
        }
        hex = hex.toLowerCase();
        return new Color((int) Long.parseLong(hex, 16));
    }
}
