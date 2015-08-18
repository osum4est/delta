package com.eightbitforest.com.objects;

import com.badlogic.gdx.physics.box2d.Body;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.badlogic.gdx.physics.box2d.FixtureDef;
import com.badlogic.gdx.physics.box2d.Shape;

/**
 * Created by osumf on 8/17/2015.
 */
public abstract class PhysicsGameObject extends GameObject {

    public Body body;

    protected abstract Body getBody(BodyDef bdef, FixtureDef fdef);

    public PhysicsGameObject()
    {
        super();
        body = getBody(new BodyDef(), new FixtureDef());
    }

    public PhysicsGameObject(String image)
    {
        super(image);
        body = getBody(new BodyDef(), new FixtureDef());
    }
}
