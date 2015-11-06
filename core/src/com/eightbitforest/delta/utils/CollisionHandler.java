package com.eightbitforest.delta.utils;

import com.badlogic.gdx.physics.box2d.Contact;
import com.badlogic.gdx.physics.box2d.ContactImpulse;
import com.badlogic.gdx.physics.box2d.ContactListener;
import com.badlogic.gdx.physics.box2d.Manifold;

/**
 * Created by fjon2248 on 10/19/2015.
 */
public class CollisionHandler implements ContactListener {
    @Override
    public void beginContact(Contact contact) {
        ((BodyData) contact.getFixtureA().getBody().getUserData()).gameObject.onCollideEnter(
                ((BodyData) contact.getFixtureB().getBody().getUserData()).gameObject);
        ((BodyData) contact.getFixtureB().getBody().getUserData()).gameObject.onCollideEnter(
                ((BodyData) contact.getFixtureA().getBody().getUserData()).gameObject);
    }

    @Override
    public void endContact(Contact contact) {
        ((BodyData) contact.getFixtureA().getBody().getUserData()).gameObject.onCollideExit(
                ((BodyData) contact.getFixtureB().getBody().getUserData()).gameObject);
        ((BodyData) contact.getFixtureB().getBody().getUserData()).gameObject.onCollideExit(
                ((BodyData) contact.getFixtureA().getBody().getUserData()).gameObject);
    }

    @Override
    public void preSolve(Contact contact, Manifold oldManifold) {
        //System.out.println("preSolve");
    }

    @Override
    public void postSolve(Contact contact, ContactImpulse impulse) {
        //System.out.println("postSolve");
    }
}
