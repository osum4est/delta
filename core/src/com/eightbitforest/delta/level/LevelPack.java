package com.eightbitforest.delta.level;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.files.FileHandle;

import java.util.ArrayList;
import java.util.Collections;

public class LevelPack {
    private String name;
    private ArrayList<String> levels;
    private int currentLevel;

    public LevelPack(String name) {
        this.name = name;
        FileHandle folder = Gdx.files.internal("levels/" + name);
        levels = new ArrayList<String>();
        for (FileHandle handle : folder.list()) {
            if (!handle.name().equals(".pack.json")) {
                levels.add(handle.name());
            }
        }
        Collections.sort(levels);

        currentLevel = -1;
    }

    public Level reloadCurrentLevel() {
        return LevelLoader.loadLocalLevel(name, levels.get(currentLevel));
    }

    public Level loadNextLevel() {
        currentLevel++;
        return LevelLoader.loadLocalLevel(name, levels.get(currentLevel));
    }
}
