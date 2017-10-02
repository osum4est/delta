package com.eightbitforest.delta.physics;

import com.badlogic.gdx.physics.box2d.Contact;
import com.eightbitforest.delta.objects.base.GameObject;

/**
 * Created by fjon2248 on 10/21/2015.
 */
public class DeltaContact {
    public Contact contact;
    public BodyData bodyDataA;
    public BodyData bodyDataB;
    public GameObject gameObjectA;
    public GameObject gameObjectB;

    public DeltaContact(Contact contact) {
        this.contact = contact;

        bodyDataA = (BodyData) contact.getFixtureA().getBody().getUserData();
        bodyDataB = (BodyData) contact.getFixtureB().getBody().getUserData();
        gameObjectA = bodyDataA.gameObject;
        gameObjectB = bodyDataB.gameObject;
    }

    public boolean testCollision(int idA, int idB) {
        return (gameObjectA.getId() == idA ||
                gameObjectA.getId() == idB) &&
                (gameObjectB.getId() == idA ||
                        gameObjectB.getId() == idB);

    }
}
