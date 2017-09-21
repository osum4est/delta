package com.eightbitforest.delta.utils;

import com.badlogic.gdx.physics.box2d.Contact;
import com.badlogic.gdx.physics.box2d.ContactImpulse;
import com.badlogic.gdx.physics.box2d.ContactListener;
import com.badlogic.gdx.physics.box2d.Manifold;
import com.eightbitforest.delta.objects.base.GameObjectPolygon;

import java.util.ArrayList;

public class CollisionHandler implements ContactListener {
    private ArrayList<GameObjectPolygon> beginCollisions;
    private ArrayList<GameObjectPolygon> endCollisions;

    public CollisionHandler() {
        beginCollisions = new ArrayList<GameObjectPolygon>();
        endCollisions = new ArrayList<GameObjectPolygon>();
    }

    @Override
    public void beginContact(Contact contact) {
        beginCollisions.add(((BodyData) contact.getFixtureA().getBody().getUserData()).gameObject);
        beginCollisions.add(((BodyData) contact.getFixtureB().getBody().getUserData()).gameObject);
    }

    @Override
    public void endContact(Contact contact) {
        endCollisions.add(((BodyData) contact.getFixtureA().getBody().getUserData()).gameObject);
        endCollisions.add(((BodyData) contact.getFixtureB().getBody().getUserData()).gameObject);
    }

    @Override
    public void preSolve(Contact contact, Manifold oldManifold) {
    }

    @Override
    public void postSolve(Contact contact, ContactImpulse impulse) {
    }

    public void step() {
        for (int i = 0; i < beginCollisions.size(); i += 2) {
            beginCollisions.get(i).onCollideEnter(beginCollisions.get(i + 1));
            beginCollisions.get(i + 1).onCollideEnter(beginCollisions.get(i));
        }
        beginCollisions.clear();

        for (int i = 0; i < endCollisions.size(); i += 2) {
            endCollisions.get(i).onCollideExit(endCollisions.get(i + 1));
            endCollisions.get(i + 1).onCollideExit(endCollisions.get(i));
        }
        endCollisions.clear();
    }
}
