package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.Body;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.badlogic.gdx.physics.box2d.FixtureDef;
import com.badlogic.gdx.physics.box2d.Shape;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.utils.BodyData;
import com.eightbitforest.delta.utils.Constants;

/**
 * Created by osum4est on 9/18/16.
 */
public class BodyBuilder {
    private float linearDamping = 1.5f;
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
    private ShapeBuilder shape;

    private BodyDef bodyDef;
    private FixtureDef fixtureDef;

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

    public BodyBuilder movePosition(Vector2 movement) {
        this.position = this.position.add(movement);
        return this;
    }
    public BodyBuilder setRotation(float rotation) {
        this.rotation = rotation;
        return this;
    }

    public BodyBuilder setShape(ShapeBuilder shape) {
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


    public Body createBody(Level level, GameObject go, int id) {
        Body b;
        if (shape == null)
            shape = new ShapeBuilder();
        if (bodyDef == null) {
            bodyDef = new BodyDef();
            bodyDef.type = bodyType;
            bodyDef.position.set(position);
            bodyDef.angle = rotation;
        }

        b = level.getWorld().createBody(bodyDef);
        b.setLinearDamping(linearDamping);
        b.setAngularDamping(angularDamping);
        b.setUserData(new BodyData(go, id));

        Shape[] shapes = shape.createShapes();
        for (int i = 0; i < shapes.length; i++) {
            if (fixtureDef == null) {
                fixtureDef = new FixtureDef();
                fixtureDef.density = density;
                fixtureDef.friction = friction;
                fixtureDef.restitution = restitution;
                fixtureDef.filter.categoryBits = category;
                fixtureDef.filter.maskBits = mask;
                fixtureDef.isSensor = sensor;
            }
            fixtureDef.shape = shapes[i];
            b.createFixture(fixtureDef);
            fixtureDef.shape.dispose();
        }

        return b;
    }

    public ShapeBuilder getShapeBuilder() {
        return shape;
    }
}
