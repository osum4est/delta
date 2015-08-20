package com.eightbitforest.delta.utils;

/**
 * Created by osumf on 8/17/2015.
 */
public class GameHandler {

    public static boolean debugMode = false;

    public void update(float deltaTime)
    {
        Globals.i.world.step(1 / 60f, 8, 3);

        Globals.i.camera.cameraUpdate();

        for (int i = 0; i < Globals.i.updateThese.size; i++)
        {
            Globals.i.updateThese.get(i).update(deltaTime);
        }
    }

    public void render()
    {
        Globals.i.camera.cameraRender(Globals.i.batch);

        System.out.println("rendeerer");
        for (int i = 0; i < Globals.i.updateThese.size; i++)
        {
            System.out.println("render");
            Globals.i.batch.begin();
            Globals.i.batch.setProjectionMatrix(Globals.i.camera.combined);
            Globals.i.updateThese.get(i).render(Globals.i.batch);
            Globals.i.batch.end();
        }

        if (debugMode)
            Globals.i.debugRenderer.render(Globals.i.world, Globals.i.camera.combined);
    }

    public void dispose()
    {
        for (int i = 0; i < Globals.i.updateThese.size; i++)
        {
            Globals.i.updateThese.get(i).dispose();
        }

        Globals.i.world.dispose();
        Globals.i.debugRenderer.dispose();
    }
}
