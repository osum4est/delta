package com.eightbitforest.delta.objects;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.g2d.Batch;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.graphics.g2d.ParticleEmitter;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.math.Vector2;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.GameObject;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.ObjectType;

public class Player extends GameObject {

    private float thrust = 20f;
    private float turnSpeed = .2f;

    private float force = 0f;
    private float torque = 0f;

    private boolean emitting = true;
    private ParticleEmitter thrusterEmitter;
    private ParticleEffect thrusterEffect;

    public Player(Level level, float x, float y) {
        super(level, ObjectType.PLAYER, x, y, Colors.PLAYER);

        thrusterEffect = new ParticleEffect();
        thrusterEffect.load(Gdx.files.internal("effects/thruster.p"), Gdx.files.internal("images"));
        thrusterEffect.setPosition(0, 0);
        thrusterEffect.start();
        thrusterEmitter = thrusterEffect.findEmitter("thruster");
    }

    @Override
    public void act(float delta) {
        ParticleEmitter.ScaledNumericValue angle = thrusterEmitter.getAngle();
        float degs = body.getAngle() * MathUtils.radiansToDegrees + 90;
        angle.setHigh(degs - 30, degs + 30);

        thrusterEffect.setPosition(body.getPosition().x, body.getPosition().y);

        Vector2 v2force = new Vector2(0, 1).rotateRad(body.getAngle()).nor();

        if (force > 0 && !emitting) {
            thrusterEffect.start();
            emitting = true;
        }
        else if (force <= 0 && emitting) {
            thrusterEffect.allowCompletion();
            emitting = false;
        }

        body.applyForceToCenter(new Vector2(v2force.x * force, v2force.y * force), true);
        body.applyTorque(torque, true);
        torque = 0;

        thrusterEffect.update(delta);
    }

    @Override
    public void draw(Batch batch, float parentAlpha) {
        thrusterEffect.draw(batch);
        super.draw(batch, parentAlpha);
    }

    public void startMoving() {
        this.force = thrust;
    }

    public void stopMoving() {
        this.force = 0;
    }

    public void turn(int deltaX) {
        this.torque = deltaX * -turnSpeed;
    }
}
