package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.*;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.utils.BodyData;
import com.eightbitforest.delta.utils.Constants;

/**
 * Created by osum4est on 9/18/16.
 */
public class BodyBuilder {
    private float linearDamping = 2.0f;
    private float angularDamping = 2.0f;
    private float density = 1.0f;
    private float friction = 1.0f;
    private float restitution = 0.0f;
    private short category = Constants.CATEGORY_ALL;
    private short mask = Constants.MASK_ALL;

    private boolean sensor = false;

    private BodyDef.BodyType bodyType = BodyDef.BodyType.DynamicBody;
    private Vector2 position = new Vector2(0, 0);
    private float rotation = 0f;
    private Shape shape;

    private BodyDef bodyDef;
    private FixtureDef fixtureDef;

    private float triangleSize = Constants.TRIANGLE_HEIGHT;

    public BodyBuilder setLinearDamping(float linearDamping) {
        this.linearDamping = linearDamping;
        return this;
    }
    public BodyBuilder setAngularDamping(float angularDamping) {
        this.angularDamping = angularDamping;
        return this;
    }
    public BodyBuilder setDensity(float density) {
        this.density = density;
        return this;
    }
    public BodyBuilder setFriction(float friction) {
        this.friction = friction;
        return this;
    }
    public BodyBuilder setRestitution(float restitution) {
        this.restitution = restitution;
        return this;
    }
    public BodyBuilder setCategory(short category) {
        this.category = category;
        return this;
    }
    public BodyBuilder setMask(short mask) {
        this.mask = mask;
        return this;
    }
    public BodyBuilder setSensor(boolean sensor) {
        this.sensor = sensor;
        return this;
    }
    public BodyBuilder setBodyType(BodyDef.BodyType bodyType) {
        this.bodyType = bodyType;
        return this;
    }
    public BodyBuilder setPosition(Vector2 position) {
        this.position = position;
        return this;
    }
    public BodyBuilder setRotation(float rotation) {
        this.rotation = rotation;
        return this;
    }
    public BodyBuilder setShape(Shape shape) {
        this.shape = shape;
        return this;
    }
    public BodyBuilder setBodyDef(BodyDef bodyDef) {
        this.bodyDef = bodyDef;
        return this;
    }
    public BodyBuilder setFixtureDef(FixtureDef fixtureDef) {
        this.fixtureDef = fixtureDef;
        return this;
    }
    public BodyBuilder setTriangleSize(float triangleSize) {
        this.triangleSize = triangleSize;
        return this;
    }
    public float getTriangleSize() {
        return triangleSize;
    }


    public Body createBody(Level level, GameObject go, int id) {
        Body b;
        if (shape == null)
            shape = getTriangleShape(triangleSize);
        if (bodyDef == null) {
            bodyDef = new BodyDef();
            bodyDef.type = bodyType;
            bodyDef.position.set(position);
            bodyDef.angle = rotation;
        }
        if (fixtureDef == null) {
            fixtureDef = new FixtureDef();
            fixtureDef.density = density;
            fixtureDef.friction = friction;
            fixtureDef.restitution = restitution;
            fixtureDef.shape = shape;
            fixtureDef.filter.categoryBits = category;
            fixtureDef.filter.maskBits = mask;
            fixtureDef.isSensor = sensor;
        }
        b = level.getWorld().createBody(bodyDef);
        b.createFixture(fixtureDef);
        b.setLinearDamping(linearDamping);
        b.setAngularDamping(angularDamping);
        b.setUserData(new BodyData(go, id));

        shape.dispose();

        return b;
    }

    public PolygonShape getTriangleShape() { return getTriangleShape(Constants.TRIANGLE_HEIGHT); }
    public PolygonShape getTriangleShape(float height)
    {
        float side = (2 / (float)Math.sqrt(3)) * height;

        PolygonShape shape = new PolygonShape();
        shape.set(new Vector2[]{
                new Vector2(-side / 2, -height / 3),
                new Vector2(side / 2, -height / 3),
                new Vector2(0, height / 3 * 2)});

        return shape;
    }
}
