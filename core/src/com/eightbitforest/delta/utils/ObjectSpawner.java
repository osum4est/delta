package com.eightbitforest.delta.utils;

import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.math.Vector2;
import com.eightbitforest.delta.objects.Player;
import com.eightbitforest.delta.objects.base.GameObjectDynamic;
import com.eightbitforest.delta.utils.interfaces.ISpawnable;
import com.eightbitforest.delta.utils.interfaces.IUpdates;

import java.util.Random;

public class ObjectSpawner implements IUpdates {

    public Player player;
    public Vector2 lastPlayerPos;
    public Vector2 playerPos;

    Random random = new Random();

    public ObjectSpawner()
    {
        super();

        player = G.i.player;
        lastPlayerPos = new Vector2(0, 0);
        playerPos = player.body.getPosition();
    }

    Vector2 move = new Vector2(0, 0);
    @Override
    public void update(float deltaTime) {
        move.x = 0;
        move.y = 0;

        if (playerPos.x > lastPlayerPos.x + G.i.SPAWN_SQUARE_SIZE / 4 ||
                playerPos.x < lastPlayerPos.x - G.i.SPAWN_SQUARE_SIZE / 4) {
            move.x = Math.signum(playerPos.x - lastPlayerPos.x);
            lastPlayerPos.x = playerPos.x;
        } else if (playerPos.y > lastPlayerPos.y + G.i.SPAWN_SQUARE_SIZE / 4 ||
                playerPos.y < lastPlayerPos.y - G.i.SPAWN_SQUARE_SIZE / 4) {
            move.y = Math.signum(playerPos.y - lastPlayerPos.y);
            lastPlayerPos.y = playerPos.y;
        }
        else
            return;

        int quarterSize = G.i.SPAWN_SQUARE_SIZE / 4;
        int halfSize = G.i.SPAWN_SQUARE_SIZE / 2;

        System.out.println(G.i.spawnThese.size);

        for (Class<? extends ISpawnable> spawnable : G.i.spawnThese)
        {
            ISpawnable iSpawnable;
            try {
                iSpawnable = spawnable.newInstance();

                if (iSpawnable != null) {
                    for (int i = 0; i < iSpawnable.getSpawnAmount(); i++) {
                        float chance = random.nextFloat();
                        if (chance <= iSpawnable.getSpawnChance())
                        {
                            spawnObjectAtPosition(spawnable,
                                    DeltaUtils.randomRange(quarterSize, halfSize) * move.x + DeltaUtils.randomRange(-halfSize, halfSize) * move.y + lastPlayerPos.x,
                                    DeltaUtils.randomRange(quarterSize, halfSize) * move.y + DeltaUtils.randomRange(-halfSize, halfSize) * move.x + lastPlayerPos.y);
                        }
                    }
                }

                iSpawnable.dispose();
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public void spawnObjectAtPosition(Class<? extends ISpawnable> spawnable, float x, float y) {
        try {
            ISpawnable spawnTemp = spawnable.newInstance();
            Vector2 pos = new Vector2(0, 0);
            pos.x = x;
            pos.y = y;
            spawnTemp.getBody().setTransform(pos, spawnTemp.getBody().getAngle());
            GameRegistry.registerObject(spawnTemp);
        } catch (Exception e) {
            System.out.println("ERROR: Cannot spawn object");
        }
    }

    public void spawnObjectAtPosition(GameObjectDynamic gameObjectDynamic, float x, float y) {
        try {
            Vector2 pos = new Vector2(0, 0);
            pos.x = x;
            pos.y = y;
            gameObjectDynamic.body.setTransform(pos, gameObjectDynamic.body.getAngle());
            GameRegistry.registerObject(gameObjectDynamic);
        } catch (Exception e) {
            System.out.println("ERROR: Cannot spawn object");
        }
    }

    public void spawnObjectAtPositionAndRotation(GameObjectDynamic gameObjectDynamic, float x, float y, float rotation) {
        try {
            Vector2 pos = new Vector2(0, 0);
            pos.x = x;
            pos.y = y;
            gameObjectDynamic.body.setTransform(pos, rotation);
            GameRegistry.registerObject(gameObjectDynamic);
        } catch (Exception e) {
            System.out.println("ERROR: Cannot spawn object");
        }
    }

    @Override
    public void render(SpriteBatch batch) {

    }

    @Override
    public void dispose() {

    }
}
