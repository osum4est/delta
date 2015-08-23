package com.eightbitforest.delta.utils;

import com.badlogic.gdx.math.Vector2;
import com.eightbitforest.delta.objects.Player;
import com.eightbitforest.delta.objects.Updates;
import org.reflections.Reflections;

import java.util.Iterator;
import java.util.Random;
import java.util.Set;

/**
 * Created by osumf on 8/18/2015.
 */
public class ObjectSpawner extends Updates {

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

        Reflections reflections = new Reflections("com.eightbitforest.delta");
        Set<Class<? extends ISpawnable>> spawnables = reflections.getSubTypesOf(ISpawnable.class);

        System.out.println(spawnables.size());
        for (int i = 0; i < spawnables.size(); i++)
        {
            //ISpawnable s = spawnables.i
            Iterator itr = spawnables.iterator();
            while (itr.hasNext())
            {
                G.i.spawnables.add((Class<? extends ISpawnable>) itr.next());
//                ISpawnable spawnable = null;
//                try {
//                    spawnable = sp.getDeclaredConstructor(Color.class).newInstance(Color.valueOf("FFFFFF"));
//                } catch (InstantiationException e) {
//                    e.printStackTrace();
//                } catch (IllegalAccessException e) {
//                    e.printStackTrace();
//                } catch (NoSuchMethodException e) {
//                    e.printStackTrace();
//                } catch (InvocationTargetException e) {
//                    e.printStackTrace();
//                }
//                System.out.println(spawnable.getSpawnAmount());
//                spawnable.dispose();
            }
            //System.out.println("Spawn amount: " + ((ISpawnable) spawnables.toArray()[i]).getSpawnAmount());
        }
    }

    Vector2 move = new Vector2(0, 0);
    @Override
    public void update(float deltaTime) {


        //System.out.println("LPP: " + lastPlayerPos.y);
        //System.out.println("PP: " + playerPos.y);

        move.x = 0;
        move.y = 0;

        if (Math.abs(playerPos.x) > Math.abs(lastPlayerPos.x) + G.i.SPAWN_SQUARE_SIZE * 1 / 4) {
            lastPlayerPos.x = playerPos.x;
            move.x = Math.signum(lastPlayerPos.x);
        }
        else if (Math.abs(playerPos.y) > Math.abs(lastPlayerPos.y) + G.i.SPAWN_SQUARE_SIZE * 1 / 4) {
            lastPlayerPos.y = playerPos.y;
            move.y = Math.signum(lastPlayerPos.y);
        }
        else
            return;

        int quarterSize = G.i.SPAWN_SQUARE_SIZE / 4;
        int halfSize = G.i.SPAWN_SQUARE_SIZE / 2;

        for (Class<? extends ISpawnable> spawnable : G.i.spawnables)
        {
            ISpawnable iSpawnable;
            try {
                iSpawnable = spawnable.newInstance();

                if (iSpawnable != null) {
                    for (int i = 0; i < iSpawnable.getSpawnAmount(); i++) {
                        float chance = random.nextFloat();
                        if (chance <= iSpawnable.getSpawnChance())
                        {
                            System.out.println("X: " + lastPlayerPos.x + ", Y: " + lastPlayerPos.y);
                            ISpawnable spawnTemp = spawnable.newInstance();
                            Vector2 pos = new Vector2(0, 0);
                            pos.x = DeltaUtils.randomRange(quarterSize, halfSize) * move.x + DeltaUtils.randomRange(-halfSize, halfSize) * move.y + lastPlayerPos.x;
                            pos.y = DeltaUtils.randomRange(quarterSize, halfSize) * move.y + DeltaUtils.randomRange(-halfSize, halfSize) * move.x + lastPlayerPos.y;

                            spawnTemp.getBody().setTransform(pos, spawnTemp.getBody().getAngle());
                        }
                    }
                }

                iSpawnable.dispose();
            } catch (InstantiationException e) {
                e.printStackTrace();
            } catch (IllegalAccessException e) {
                e.printStackTrace();
            }
        }

    }
}
