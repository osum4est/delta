package com.eightbitforest.delta.utils;

import com.badlogic.gdx.math.Vector2;
import com.eightbitforest.delta.objects.Player;
import com.eightbitforest.delta.objects.Updates;

/**
 * Created by osumf on 8/18/2015.
 */
public class ObjectSpawner extends Updates {

    public Player player;
    public Vector2 lastPlayerPos;
    public Vector2 playerPos;


    public ObjectSpawner()
    {
        super();

        player = G.i.player;
        lastPlayerPos = Vector2.Zero;
        playerPos = player.body.getPosition();
    }

    @Override
    public void update(float deltaTime) {

        if (Math.abs(playerPos.x) > G.i.SPAWN_SQUARE_SIZE) {
            lastPlayerPos.x = playerPos.x;
        }
        else if (Math.abs(playerPos.y) > G.i.SPAWN_SQUARE_SIZE) {
            lastPlayerPos.y = playerPos.y;
        }
        else
            return;


    }
}
