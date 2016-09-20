package com.eightbitforest.delta.objects;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.graphics.g2d.ParticleEmitter;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.utils.Timer;
import com.eightbitforest.delta.objects.base.BodyBuilder;
import com.eightbitforest.delta.objects.base.GameObjectDynamic;
import com.eightbitforest.delta.objects.base.GameObjectDynamicTriangle;
import com.eightbitforest.delta.objects.base.PlayerDespawnCollider;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.ObjectType;
import com.eightbitforest.delta.utils.interfaces.ITouchInput;
import sun.security.ssl.Debug;

/**
 * Created by osumf on 8/17/2015.
 */
public class Player extends GameObjectDynamicTriangle implements ITouchInput {

    public float thrust = 20f;
    public float turnSpeed = .2f;

    public float force = 0f;
    public float torque = 0f;

    private boolean emitting = true;
    ParticleEmitter thrusterEmitter;
    ParticleEffect thrusterEffect;

    Timer touchTimer;

    ObjectPlanet planet;

    public Player() {
        super(ObjectType.PLAYER,
                new BodyBuilder()
                    .setCategory(G.i.CATEGORY_PLAYER)
                    .setMask(G.i.MASK_PLAYER));

        thrusterEffect = new ParticleEffect();
        thrusterEffect.load(Gdx.files.internal("effects/thruster.p"), Gdx.files.internal("images"));
        thrusterEffect.setPosition(0, 0);
        thrusterEffect.start();
        thrusterEmitter = thrusterEffect.findEmitter("thruster");

        touchTimer = new Timer();


        planet = new ObjectPlanet(0, 260 / G.i.PPM);
    }

    public void shoot() {
        G.i.objectSpawner.spawnObjectAtPosition(new Bullet(.3f, body.getAngle()), body.getPosition().x, body.getPosition().y);
    }

    @Override
    public void touchDown(int screenX, int screenY, int pointer, int button) {
        touchTimer.scheduleTask(new Timer.Task() {
            @Override
            public void run() {
                force = thrust;
            }
        }, .15f);
    }

    @Override
    public void touchUp(int screenX, int screenY, int pointer, int button) {
        System.out.println(body.getPosition().scl(G.i.PPM));
        touchTimer.clear();
        if (force != thrust)
            shoot();
        force = 0;
    }

    @Override
    public void touchDragged(int screenX, int screenY, int pointer) {
        torque = Gdx.input.getDeltaX() * -turnSpeed;
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
