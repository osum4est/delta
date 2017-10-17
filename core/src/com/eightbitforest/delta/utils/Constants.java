package com.eightbitforest.delta.utils;

import com.badlogic.gdx.Gdx;

public class Constants {
    public static final boolean DEBUG_MODE = false;
    public static final String TAG = "delta";

    public static final int PPM = 15;
    public static final float TRIANGLE_SIDE = 1f; // GRID WIDTH
    public static final float TRIANGLE_HEIGHT = Utils.getTriangleHeight(TRIANGLE_SIDE); // GRID HEIGHT

    public static final float RESTART_DELAY = 1f;

    public static final short CATEGORY_NONE = 0x1;
    public static final short CATEGORY_ALL = 0x2;
    public static final short CATEGORY_PLAYER = 0x4;
    public static final short CATEGORY_ENEMY = 0x8;
    public static final short MASK_NONE = 0x0;
    public static final short MASK_ALL = 0xF;
    public static final short MASK_PLAYER = CATEGORY_ENEMY | CATEGORY_ALL;
    public static final short MASK_ENEMY = CATEGORY_PLAYER | CATEGORY_ALL;

    public static final float VIEWPORT_WIDTH = 320;
    public static final float VIEWPORT_HEIGHT = 480;

    public static final float WINDOW_WIDTH = Gdx.graphics.getWidth();
    public static final float WINDOW_HEIGHT = Gdx.graphics.getHeight();

    public static final float CAMERA_LERP_AMOUNT = .1f;

    public static final float PLAYER_THRUST = 20f;
    public static final float PLAYER_TURN_SPEED = 50f;
    public static final float PLAYER_MAX_DV = 5f;

    public static final float ASTEROID_VARIATION = .05f;

    public static final float HUD_DV_HEIGHT = 5f;
    public static final float TEXTURE_RESOLUTION = 1000;
}

