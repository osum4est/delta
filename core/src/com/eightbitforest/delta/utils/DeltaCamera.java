package com.eightbitforest.delta.utils;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.Globals;

public class DeltaCamera extends OrthographicCamera {

    public float moveSpeed = .05f;

    ParticleEffect starsEffect;

    public DeltaCamera()
    {
        super(Gdx.graphics.getWidth() / G.i.CAMERA_SIZE, Gdx.graphics.getHeight() / G.i.CAMERA_SIZE);

        starsEffect = new ParticleEffect();
        starsEffect.load(Gdx.files.internal("effects/stars.p"), Gdx.files.internal("images"));
        starsEffect.setPosition(0, 0);
        starsEffect.scaleEffect(1 / (float) G.i.CAMERA_SIZE);
        starsEffect.start();
    }

    public void cameraUpdate()
    {
        Vector3 pos = position;
        Vector2 v = new Vector2(pos.x, pos.y);
        v.lerp(Globals.i.player.body.getPosition(), moveSpeed);
        position.set(new Vector3(v.x, v.y, pos.z));
        update();

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
