package com.eightbitforest.delta.utils;

import com.eightbitforest.delta.objects.base.GameObjectDynamic;

/**
 * Created by fjon2248 on 10/19/2015.
 */
public class BodyData {

    public GameObjectDynamic gameObject;
    public int id;

    public BodyData(GameObjectDynamic gameObject, int id) {
        this.gameObject = gameObject;
        this.id = id;
    }

}
