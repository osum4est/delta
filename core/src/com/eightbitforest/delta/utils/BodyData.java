package com.eightbitforest.delta.utils;

import com.eightbitforest.delta.objects.base.GameObjectPolygon;

/**
 * Created by fjon2248 on 10/19/2015.
 */
public class BodyData {

    public GameObjectPolygon gameObject;
    public int id;

    public BodyData(GameObjectPolygon gameObject, int id) {
        this.gameObject = gameObject;
        this.id = id;
    }

}
