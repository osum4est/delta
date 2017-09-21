package com.eightbitforest.delta.views;

import com.badlogic.gdx.Game;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Screen;
import com.badlogic.gdx.graphics.g2d.ParticleEffect;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.physics.box2d.Box2DDebugRenderer;
import com.eightbitforest.delta.debug.DebugGrid;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.level.LevelPack;
import com.eightbitforest.delta.ui.InGameHud;
import com.eightbitforest.delta.utils.Constants;

public class MainGame implements Screen {
    private Game game;
    private LevelPack levelPack;
    private Level currentLevel;
    private InGameHud hud;

    private ParticleEffect starsEffect;

    private Box2DDebugRenderer debugRenderer;
    private DebugGrid debugGrid;

    private static MainGame instance;

    public MainGame(Game game) {
        instance = this;

        this.game = game;

        levelPack = new LevelPack("pack_001");
        switchLevel(levelPack.loadNextLevel());

        starsEffect = new ParticleEffect();
        starsEffect.load(Gdx.files.internal("effects/stars.p"), Gdx.files.internal("images"));
        starsEffect.setPosition(0, 0);
        starsEffect.start();

        if (Constants.DEBUG_MODE) {
            debugRenderer = new Box2DDebugRenderer();
            debugGrid = new DebugGrid();
        }
    }

    public static MainGame getInstance() {
        return instance;
    }

    @Override
    public void show() {

    }

    @Override
    public void render(float delta) {
        currentLevel.getCamera().position.x = MathUtils.lerp(currentLevel.getCamera().position.x, currentLevel.getPlayer().getX(), Constants.CAMERA_LERP_AMOUNT);
        currentLevel.getCamera().position.y = MathUtils.lerp(currentLevel.getCamera().position.y, currentLevel.getPlayer().getY(), Constants.CAMERA_LERP_AMOUNT);

        starsEffect.update(delta);
        starsEffect.setPosition(currentLevel.getCamera().position.x, currentLevel.getCamera().position.y);
        currentLevel.getBatch().begin();
        starsEffect.draw(currentLevel.getBatch());
        currentLevel.getBatch().end();

        currentLevel.act();
        currentLevel.draw();

        if (Constants.DEBUG_MODE) {
            debugGrid.render(currentLevel.getBatch().getProjectionMatrix(), 25, 25);
            debugRenderer.render(currentLevel.getWorld(), currentLevel.getBatch().getProjectionMatrix());
        }
    }

    public void nextLevel() {
        switchLevel(levelPack.loadNextLevel());
    }

    public void switchLevel(Level level) {
        if (currentLevel != null)
            currentLevel.dispose();

        currentLevel = level;
        hud = new InGameHud(currentLevel);
        currentLevel.addActor(hud);
        Gdx.input.setInputProcessor(currentLevel);
        currentLevel.getCamera().position.x = 0;
        currentLevel.getCamera().position.y = 0;
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
        currentLevel.dispose();
    }
}
