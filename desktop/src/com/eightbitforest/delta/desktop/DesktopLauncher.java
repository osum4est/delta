package com.eightbitforest.delta.desktop;

import com.badlogic.gdx.backends.lwjgl.LwjglApplication;
import com.badlogic.gdx.backends.lwjgl.LwjglApplicationConfiguration;
import com.eightbitforest.delta.DeltaMain;

public class DesktopLauncher {
	public static void main (String[] arg) {
        LwjglApplicationConfiguration config = new LwjglApplicationConfiguration();
//        config.setWindowedMode(640, 480 * 2);
        config.height = 480 * 2;
        config.width = 640;
        new LwjglApplication(new DeltaMain(), config);
    }
}
