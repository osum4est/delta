package com.eightbitforest.delta.utils;

public class Constants {
    public static final boolean DEBUG_MODE = true;
    public static final String TAG = "delta";


    public static final int PPM = 15;
    public static final float TRIANGLE_SIDE = 1f; // GRID WIDTH
    public static final float TRIANGLE_HEIGHT = TRIANGLE_SIDE / (2 / (float) Math.sqrt(3)); // GRID HEIGHT

    public static final short CATEGORY_NONE = 0x1;
    public static final short CATEGORY_ALL = 0x2;
    public static final short CATEGORY_PLAYER = 0x4;
    public static final short CATEGORY_ENEMY = 0x8;
    public static final short MASK_NONE = 0x0;
    public static final short MASK_ALL = 0xF;
    public static final short MASK_PLAYER = CATEGORY_ENEMY | CATEGORY_ALL;
    public static final short MASK_ENEMY = CATEGORY_PLAYER | CATEGORY_ALL;

    public static final float WIDTH = 320;
    public static final float HEIGHT = 480;

    public static final float CAMERA_LERP_AMOUNT = .1f;
}
