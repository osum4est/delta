package com.eightbitforest.com;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.math.Vector3;
import com.eightbitforest.com.utils.Globals;

/**
 * Created by osumf on 8/17/2015.
 */
public class DeltaCamera extends OrthographicCamera {

    public float moveSpeed = .05f;

    public DeltaCamera()
    {
        super(Gdx.graphics.getWidth() / Globals.CAMERA_SIZE, Gdx.graphics.getHeight() / Globals.CAMERA_SIZE);
    }

    public void cameraUpdate()
    {
        Vector3 pos = position;
        Vector2 v = new Vector2(pos.x, pos.y);
        v.lerp(Globals.i.player.body.getPosition(), moveSpeed);
        position.set(new Vector3(v.x, v.y, pos.z));
        update();
    }
}
