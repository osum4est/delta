package com.eightbitforest.delta.objects;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.graphics.g2d.ParticleEmitter;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.math.Vector2;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.ITouchInput;
import com.eightbitforest.delta.utils.ObjectType;

/**
 * Created by osumf on 8/17/2015.
 */
public class Player extends GameObjectDynamicTriangle implements ITouchInput {

    public float thrust = 15f;
    public float turnSpeed = .25f;

    public float force = 0f;
    public float torque = 0f;

    private boolean emitting = true;
    ParticleEmitter thrusterEmitter;
    ParticleEffect thrusterEffect;

    public Player() {
        thrusterEffect = new ParticleEffect();
        thrusterEffect.load(Gdx.files.internal("effects/thruster.p"), Gdx.files.internal("images"));
        thrusterEffect.setPosition(0, 0);
        thrusterEffect.scaleEffect(1 / (float) G.i.CAMERA_SIZE);
        thrusterEffect.start();

        thrusterEmitter = thrusterEffect.findEmitter("thruster");

        G.i.inputThese.add(this);
    }

    @Override
    public void touchDown(int screenX, int screenY, int pointer, int button) {
        force = thrust;
    }

    @Override
    public void touchUp(int screenX, int screenY, int pointer, int button) {
        force = 0;
    }

    @Override
    public void touchDragged(int screenX, int screenY, int pointer) {
        torque = Gdx.input.getDeltaX() * -turnSpeed;
    }

    @Override
    int getId() {
        return ObjectType.PLAYER;
    }

    @Override
    public Color getColor() {
        return Color.valueOf("2EDCE8");
    }

    @Override
    public void update(float deltaTime) {

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

        thrusterEffect.update(deltaTime);
    }


    @Override
    public void render(SpriteBatch batch) {
        thrusterEffect.draw(batch);
        super.render(batch);
    }
}
