package com.eightbitforest.com.desktop;

import com.badlogic.gdx.backends.lwjgl.LwjglApplication;
import com.badlogic.gdx.backends.lwjgl.LwjglApplicationConfiguration;
import com.eightbitforest.com.DeltaMain;

public class DesktopLauncher {
	public static void main (String[] arg) {
		LwjglApplicationConfiguration config = new LwjglApplicationConfiguration();

		config.width = 400;
		config.height = 640;

		config.samples = 4;

		new LwjglApplication(new DeltaMain(), config);
	}
}
