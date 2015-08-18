package com.eightbitforest.com;

import com.badlogic.gdx.ApplicationAdapter;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;
import com.badlogic.gdx.physics.box2d.*;
import com.eightbitforest.com.utils.GameHandler;
import com.eightbitforest.com.utils.Globals;
import com.eightbitforest.com.utils.InputHandler;

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
	public void dispose() {

		Globals.i.gameHandler.dispose();
	}
}
