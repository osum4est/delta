package com.eightbitforest.delta.utils;

import com.eightbitforest.delta.objects.GameObjectDynamicTriangle;

/**
 * Created by fjon2248 on 10/19/2015.
 */
public class BodyData {

    public GameObjectDynamicTriangle gameObject;
    public int id;

    public BodyData(GameObjectDynamicTriangle gameObject, int id) {
        this.gameObject = gameObject;
        this.id = id;
    }

}
