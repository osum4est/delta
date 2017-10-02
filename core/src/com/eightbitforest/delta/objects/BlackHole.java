package com.eightbitforest.delta.objects;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.Batch;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.BodyBuilder;
import com.eightbitforest.delta.objects.base.GameObject;
import com.eightbitforest.delta.objects.base.ShapeBuilder;
import com.eightbitforest.delta.utils.Ids;

public class BlackHole extends GameObject {
    private static float effectRadius = 5f;
    private static float pullForce = 2f;

    // TODO: Make effect start generated
    private ParticleEffect blackHoleEffect;

    public BlackHole(Level level, float x, float y) {
        super(level, Ids.BLACK_HOLE, x, y, Color.BLACK, false);
        setBody(new BodyBuilder().setBodyType(BodyDef.BodyType.StaticBody).setSensor(true)
                .setShape(new ShapeBuilder().setAsCircle(.4f)));

        blackHoleEffect = new ParticleEffect();
        blackHoleEffect.load(Gdx.files.internal("effects/black_hole.p"), Gdx.files.internal("images"));
        blackHoleEffect.setPosition(getX(), getY());
        blackHoleEffect.start();
    }

    @Override
    public void act(float delta) {
        super.act(delta);

        // Drag player towards center
        Vector2 diff = getLevel().getPlayer().getBody().getTransform().getPosition().sub(body.getTransform().getPosition());
        if (diff.len() < effectRadius) {
            diff.nor();
            diff.scl(-5f);
            getLevel().getPlayer().getBody().applyForceToCenter(diff, true);
        }

        blackHoleEffect.update(delta);
    }

    @Override
    public void draw(Batch batch, float parentAlpha) {
        blackHoleEffect.draw(batch);
    }
}
