package com.eightbitforest.delta.objects;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.g2d.Batch;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.graphics.g2d.ParticleEmitter;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.math.Vector2;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.GameObjectPolygon;
import com.eightbitforest.delta.objects.walls.Wall;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.Constants;
import com.eightbitforest.delta.utils.Ids;

public class Player extends GameObjectPolygon {
    private float force = 0f;
    private float torque = 0f;

    private float dV = Constants.PLAYER_MAX_DV;

    private boolean emitting = true;
    private ParticleEmitter thrusterEmitter;
    private ParticleEffect thrusterEffect;

    private boolean dead = false;

    public Player(Level level, float x, float y) {
        super(level, Ids.PLAYER, x, y, Colors.PLAYER);

        thrusterEffect = new ParticleEffect();
        thrusterEffect.load(Gdx.files.internal("effects/thruster.p"), Gdx.files.internal("images"));
        thrusterEffect.setPosition(0, 0);
        thrusterEffect.start();
        thrusterEmitter = thrusterEffect.findEmitter("thruster");
    }

    @Override
    public void act(float delta) {
        if (dV > 0 && !dead) {
            if (force > 0)
                dV -= delta;

            Vector2 v2force = new Vector2(0, 1).rotateRad(body.getAngle()).nor();
            body.applyForceToCenter(new Vector2(v2force.x * force, v2force.y * force), true);
            body.applyTorque(torque, true);
            torque = 0;
        } else if (force > 0) {
            force = 0;
        }

        if (force > 0 && !emitting) {
            thrusterEffect.start();
            emitting = true;
        } else if (force <= 0 && emitting) {
            thrusterEffect.allowCompletion();
            emitting = false;
        }

        ParticleEmitter.ScaledNumericValue angle = thrusterEmitter.getAngle();
        float degs = body.getAngle() * MathUtils.radiansToDegrees + 90;
        angle.setHigh(degs - 30, degs + 30);
        thrusterEffect.setPosition(body.getPosition().x, body.getPosition().y);
        thrusterEffect.update(delta);
    }

    @Override
    public void draw(Batch batch, float parentAlpha) {
        if (!dead) {
            thrusterEffect.draw(batch);
            super.draw(batch, parentAlpha);
        }
    }

    public void startMoving() {
        if (dV > 0) {
            this.force = Constants.PLAYER_THRUST;
        }
    }

    public void stopMoving() {
        this.force = 0;
    }

    public void turn(float deltaX) {
        this.torque = deltaX * -Constants.PLAYER_TURN_SPEED;
    }

    public float getCurrentDV() {
        return dV;
    }

    public void die() {
        dead = true;
        body.setActive(false);
        PlayerDeathPart[] deathParts = new PlayerDeathPart[3];
        float angle = (float) Math.random() * 180;
        for (int i = 0; i < deathParts.length; i++) {
            deathParts[i] = new PlayerDeathPart(getLevel(), Ids.PLAYER, getX(), getY());
            getLevel().addActor(deathParts[i]);

            Vector2 force = new Vector2(0, 250);
            force.rotate(angle + i * 120);
            deathParts[i].getBody().applyForceToCenter(force, true);
            deathParts[i].getBody().applyTorque((float) Math.random() * 120 - 60, true);
        }
    }

    @Override
    public void onCollideEnter(GameObjectPolygon other) {
        if (other instanceof Wall && !dead) {
            die();
        }
    }

    private static class PlayerDeathPart extends GameObjectPolygon {

        public PlayerDeathPart(Level level, int id, float x, float y) {
            super(level, id, x, y, Colors.PLAYER, Constants.TRIANGLE_SIDE / 2);
        }
    }
}
