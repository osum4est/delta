package com.eightbitforest.delta.views;

import com.badlogic.gdx.Game;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.physics.box2d.Box2DDebugRenderer;
import com.eightbitforest.delta.debug.DebugGrid;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.level.LevelLoader;
import com.eightbitforest.delta.ui.InGameHud;
import com.eightbitforest.delta.utils.Constants;

public class MainGame implements Screen {

    private Game game;
    private Level level;
    private InGameHud hud;

    private ParticleEffect starsEffect;

    private Box2DDebugRenderer debugRenderer;
    private DebugGrid debugGrid;

    public MainGame(Game game) {
        this.game = game;
        this.level = LevelLoader.loadLocalLevel("001.lvl");
        this.hud = new InGameHud(this.level);
        level.addActor(this.hud);

        Gdx.input.setInputProcessor(level);

        starsEffect = new ParticleEffect();
        starsEffect.load(Gdx.files.internal("effects/stars.p"), Gdx.files.internal("images"));
        starsEffect.setPosition(0, 0);
        starsEffect.start();

        level.getCamera().position.x = 0;
        level.getCamera().position.y = 0;

        if (Constants.DEBUG_MODE) {
            debugRenderer = new Box2DDebugRenderer();
            debugGrid = new DebugGrid();
        }
    }

    @Override
    public void show() {

    }

    @Override
    public void render(float delta) {
        level.getCamera().position.x = MathUtils.lerp(level.getCamera().position.x, level.getPlayer().getX(), Constants.CAMERA_LERP_AMOUNT);
        level.getCamera().position.y = MathUtils.lerp(level.getCamera().position.y, level.getPlayer().getY(), Constants.CAMERA_LERP_AMOUNT);

        starsEffect.update(delta);
        starsEffect.setPosition(level.getCamera().position.x, level.getCamera().position.y);
        level.getBatch().begin();
        starsEffect.draw(level.getBatch());
        level.getBatch().end();

        level.act();
        level.draw();

        if (Constants.DEBUG_MODE) {
            debugGrid.render(level.getBatch().getProjectionMatrix(), 25, 25);
            debugRenderer.render(level.getWorld(), level.getBatch().getProjectionMatrix());
        }
    }

    @Override
    public void resize(int width, int height) {

    }

    @Override
    public void pause() {

    }

    @Override
    public void resume() {

    }

    @Override
    public void hide() {

    }

    @Override
    public void dispose() {
        level.dispose();
    }
}
