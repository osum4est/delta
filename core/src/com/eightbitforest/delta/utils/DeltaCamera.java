package com.eightbitforest.delta.utils;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.utils.viewport.FitViewport;
import com.badlogic.gdx.utils.viewport.Viewport;

public class DeltaCamera extends OrthographicCamera {

    public float moveSpeed = .05f;

    ParticleEffect starsEffect;

    public Viewport v;

    public DeltaCamera()
    {
        v = new FitViewport(360 / G.i.PPM, 640 / G.i.PPM, this);
        v.apply();
        starsEffect = new ParticleEffect();
        starsEffect.load(Gdx.files.internal("effects/stars.p"), Gdx.files.internal("images"));
        starsEffect.setPosition(0, 0);
        starsEffect.start();
    }

    public void resize(int width, int height) {
        v.update(width, height);
    }

    public void cameraUpdate()
    {
//        Vector3 pos = position;
//        Vector2 v = new Vector2(pos.x, pos.y);
//        v.lerp(Globals.i.player.body.getPosition(), moveSpeed);
//        position.set(new Vector3(v.x, v.y, pos.z));
        update();
//        v.update((int)viewportWidth, (int)viewportHeight);

        starsEffect.setPosition(this.position.x, this.position.y);
        starsEffect.update(Gdx.graphics.getDeltaTime());
    }

    public void cameraRender(SpriteBatch batch)
    {
        batch.begin();
        batch.setProjectionMatrix(combined);
        starsEffect.draw(batch);
        batch.end();
    }
}
