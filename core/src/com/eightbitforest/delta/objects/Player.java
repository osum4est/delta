package com.eightbitforest.delta.objects;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.graphics.g2d.ParticleEmitter;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.Body;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.badlogic.gdx.physics.box2d.FixtureDef;
import com.badlogic.gdx.physics.box2d.PolygonShape;
import com.eightbitforest.delta.utils.Globals;

/**
 * Created by osumf on 8/17/2015.
 */
public class Player extends PhysicsGameObject {

    public float thrust = 15f;
    public float turnSpeed = .25f;

    public float force = 0f;
    public float torque = 0f;

    private boolean emitting = true;
    ParticleEmitter thrusterEmitter;
    ParticleEffect thusterEffect;

    @Override
    public void create() {
        thusterEffect = new ParticleEffect();
        thusterEffect.load(Gdx.files.internal("effects/thruster.p"), Gdx.files.internal("images"));
        thusterEffect.setPosition(0, 0);
        thusterEffect.scaleEffect(1 / (float) Globals.CAMERA_SIZE);
        thusterEffect.start();

        thrusterEmitter = thusterEffect.findEmitter("thruster");
    }

    @Override
    public void update(float deltaTime) {

        ParticleEmitter.ScaledNumericValue angle = thrusterEmitter.getAngle();
        float degs = body.getAngle() * MathUtils.radiansToDegrees + 90;
        angle.setHigh(degs - 30, degs + 30);

        thusterEffect.setPosition(body.getPosition().x, body.getPosition().y);

        Vector2 v2force = new Vector2(0, 1).rotateRad(body.getAngle()).nor();

        if (force > 0 && !emitting) {
            thusterEffect.start();
            emitting = true;
        }
        else if (force <= 0 && emitting) {
            thusterEffect.allowCompletion();
            emitting = false;
        }

        body.applyForceToCenter(new Vector2(v2force.x * force, v2force.y * force), true);
        body.applyTorque(torque, true);
        torque = 0;

        thusterEffect.update(deltaTime);
    }

    @Override
    public void render(SpriteBatch batch) {
        thusterEffect.draw(batch);
        batch.end();
        Globals.i.drawTriangle(Globals.TRIANGLE_HEIGHT, Color.valueOf("00B5FFFF"));
        batch.begin();
    }

    @Override
    protected Body getBody(BodyDef bdef, FixtureDef fdef) {
        Body body;

        bdef.type = BodyDef.BodyType.DynamicBody;
        bdef.position.set(0, 0);

        PolygonShape shape = Globals.i.getTriangleShape(Globals.TRIANGLE_HEIGHT);

        fdef.density = 1.0f;
        fdef.friction = 1.0f;
        fdef.restitution = 0.0f;
        fdef.shape = shape;

        body = Globals.i.world.createBody(bdef);
        body.createFixture(fdef);
        shape.dispose();

        body.setLinearDamping(2.0f);
        body.setAngularDamping(2.0f);

        return body;
    }
}
