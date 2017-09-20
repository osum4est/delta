package com.eightbitforest.delta.ui;

import com.badlogic.gdx.graphics.g2d.Batch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.Constants;

public class InGameHud extends Actor {
    private Level level;
    private ShapeRenderer shapeRenderer;

    public InGameHud(Level level) {
        this.level = level;
        shapeRenderer = new ShapeRenderer();
    }

    @Override
    public void draw(Batch batch, float parentAlpha) {
        drawPlayerDV(batch);
    }

    /**
     * Draws the players dV as a bar on top of the screen
     */
    private void drawPlayerDV(Batch batch) {
        batch.end();

        float percent = level.getPlayer().getCurrentDV() / Constants.PLAYER_MAX_DV;

        shapeRenderer.begin(ShapeRenderer.ShapeType.Filled);
        shapeRenderer.setColor(Colors.WHITE);
        shapeRenderer.rect(0, Constants.WINDOW_HEIGHT - Constants.HUD_DV_HEIGHT, Constants.WINDOW_WIDTH * percent, Constants.HUD_DV_HEIGHT);
        shapeRenderer.end();

        batch.begin();
    }
}
