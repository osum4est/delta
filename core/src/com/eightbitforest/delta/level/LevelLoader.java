package com.eightbitforest.delta.level;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.files.FileHandle;
import com.eightbitforest.delta.objects.walls.*;
import com.eightbitforest.delta.utils.Constants;

import java.util.ArrayList;

public class LevelLoader {
    public static Level loadLocalLevel(String filename) {
        FileHandle handle = Gdx.files.internal("levels/" + filename);
        String file = handle.readString();
        return loadFromString(file);
    }

    private static Level loadFromString(String levelString) {
        Level level = new Level();

        String[] lines = levelString.split("\n");
        ArrayList<String> levelLines = new ArrayList<String>();
        ArrayList<String> propLines = new ArrayList<String>();

        for (int y = 0; y < lines.length; y++) {
            lines[y] = lines[y].replace("\r", "");
            lines[y] = lines[y].replace("\n", "");
            if (!lines[y].isEmpty()) {
                if (lines[y].contains("{")) {
                    propLines.add(lines[y]);
                } else {
                    levelLines.add(lines[y]);
                }
            }
        }

        for (int y = 0; y < levelLines.size(); y++) {
            for (int x = 0; x < levelLines.get(y).length(); x++) {
                char c = levelLines.get(y).charAt(x);
                addGameObject(level, c, x, levelLines.size() - 1 - y);
            }
        }

        for (int i = 0; i < propLines.size(); i++) {
//            JsonValue json = new JsonReader().parse(propLines.get(i));
//            if (json.has("x") && json.has("y")) {
//                ArrayList<GameObjectPolygon> objects = level.getObjectsAt(
//                        json.getInt("x") - 1,
//                        levelLines.size() - json.getInt("y"));
//                if (objects.size() == 1) {
//                    objects.get(0).setProperties(json);
//                } else {
//                    System.out.println("No object at property position");
//                }
//            } else {
//                System.out.println("Level property missing x and y");
//            }
        }

//        for (ArrayList<ArrayList<GameObjectPolygon>> col : level) {
//            for (ArrayList<GameObjectPolygon> objects : col) {
//                for (GameObjectPolygon object : objects) {
//                    object.onUpdate();
//                    object.setZIndex(object.getZ());
//                }
//            }
//        }

        return level;
    }

    private static void addGameObject(Level level, char c, int x, int y) {
        switch (c) {
            case '▲':
                level.addObject(new WallTriangleUp(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
            case '▼':
                level.addObject(new WallTriangleDown(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
            case '◢':
                level.addObject(new WallTriangleHalfTopLeft(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
            case '◤':
                level.addObject(new WallTriangleHalfBottomRight(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
            case '◥':
                level.addObject(new WallTriangleHalfBottomLeft(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
            case '◣':
                level.addObject(new WallTriangleHalfTopRight(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
            case '◿':
                level.addObject(new WallTriangleHalfTopLeftLong(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
            case '◸':
                level.addObject(new WallTriangleHalfBottomRightLong(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
            case '◹':
                level.addObject(new WallTriangleHalfBottomLeftLong(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
            case '◺':
                level.addObject(new WallTriangleHalfTopRightLong(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
            case '■':
                level.addObject(new WallBox(level, x, y * Constants.TRIANGLE_HEIGHT));
                break;
        }
    }
}
