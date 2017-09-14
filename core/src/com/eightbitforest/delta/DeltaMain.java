package com.eightbitforest.delta;

import com.badlogic.gdx.Game;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.Constants;
import com.eightbitforest.delta.views.MainGame;

public class DeltaMain extends Game {

	@Override
	public void create () {
	    setScreen(new MainGame(this));
	}

	@Override
	public void render () {
		Gdx.gl.glClearColor(
                Colors.BACKGROUND.r,
                Colors.BACKGROUND.g,
                Colors.BACKGROUND.b,
                Colors.BACKGROUND.a);
		Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);

        super.render();
	}

	@Override
	public void dispose() {
        super.dispose();
	}
}
