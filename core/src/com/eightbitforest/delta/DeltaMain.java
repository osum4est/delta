package com.eightbitforest.delta;

import com.badlogic.gdx.ApplicationAdapter;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL20;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.GameHandler;
import com.eightbitforest.delta.utils.Globals;
import com.eightbitforest.delta.utils.InputHandler;

public class DeltaMain extends ApplicationAdapter {
	
	@Override
	public void create () {

		// DEBUG MODE
		GameHandler.debugMode = true;

		Globals.i.init();
		Gdx.input.setInputProcessor(new InputHandler());
	}

	@Override
	public void render () {
		Gdx.gl.glClearColor(.1f, .1f, .1f, 1);
		Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);

		Globals.i.gameHandler.update(Gdx.graphics.getDeltaTime());
		Globals.i.gameHandler.render();
	}

	@Override
	public void resize(int width, int height) {
		G.i.camera.resize(width, height);
	}

	@Override
	public void dispose() {

		Globals.i.gameHandler.dispose();
	}
}
