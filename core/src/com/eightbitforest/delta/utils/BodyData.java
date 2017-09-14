package com.eightbitforest.delta.utils;

import com.eightbitforest.delta.objects.base.GameObject;

/**
 * Created by fjon2248 on 10/19/2015.
 */
public class BodyData {

    public GameObject gameObject;
    public int id;

    public BodyData(GameObject gameObject, int id) {
        this.gameObject = gameObject;
        this.id = id;
    }

}
