package com.eightbitforest.delta.utils;

/**
 * Created by osumf on 8/17/2015.
 */
public class GameHandler {

    public static boolean debugMode = false;

    public void update(float deltaTime)
    {
        G.i.world.step(1 / 60f, 8, 3);

        G.i.camera.cameraUpdate();

        for (int i = 0; i < G.i.updateThese.size; i++)
        {
            IUpdates updates = G.i.updateThese.get(i);
            updates.update(deltaTime);
        }

        for (int i = 0; i < G.i.removeThese.size; i++) {
            G.i.world.destroyBody(G.i.removeThese.get(i).body);
            G.i.removeThese.get(i).dispose();
            G.i.removeThese.removeIndex(i);
        }
    }

    public void render()
    {
        G.i.camera.cameraRender(G.i.batch);

        for (int i = 0; i < G.i.updateThese.size; i++)
        {
            G.i.batch.begin();
            G.i.batch.setProjectionMatrix(G.i.camera.combined);
            G.i.updateThese.get(i).render(G.i.batch);
            G.i.batch.end();
        }

        if (debugMode)
            G.i.debugRenderer.render(G.i.world, G.i.camera.combined);
    }

    public void dispose()
    {
        for (int i = 0; i < G.i.updateThese.size; i++)
        {
            G.i.updateThese.get(i).dispose();
        }

        G.i.world.dispose();
        G.i.debugRenderer.dispose();
    }
}
