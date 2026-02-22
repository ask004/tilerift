from __future__ import annotations

from dataclasses import dataclass, field
from datetime import date
import hashlib
import json
import random
from typing import Dict, List, Optional, Tuple


Coord = Tuple[int, int]


@dataclass
class Board:
    width: int
    height: int
    cells: List[List[Optional[str]]] = field(init=False)

    def __post_init__(self) -> None:
        self.cells = [[None for _ in range(self.width)] for _ in range(self.height)]

    def in_bounds(self, x: int, y: int) -> bool:
        return 0 <= x < self.width and 0 <= y < self.height

    def get(self, x: int, y: int) -> Optional[str]:
        if not self.in_bounds(x, y):
            raise IndexError("out of bounds")
        return self.cells[y][x]

    def set(self, x: int, y: int, piece: Optional[str]) -> None:
        if not self.in_bounds(x, y):
            raise IndexError("out of bounds")
        self.cells[y][x] = piece

    def is_empty(self, x: int, y: int) -> bool:
        return self.get(x, y) is None

    def can_place(self, x: int, y: int, piece: str) -> bool:
        return self.in_bounds(x, y) and self.is_empty(x, y) and bool(piece)

    def place(self, x: int, y: int, piece: str) -> bool:
        if not self.can_place(x, y, piece):
            return False
        self.set(x, y, piece)
        return True

    def move(self, source: Coord, target: Coord) -> bool:
        sx, sy = source
        tx, ty = target
        if not self.in_bounds(sx, sy) or not self.in_bounds(tx, ty):
            return False
        piece = self.get(sx, sy)
        if piece is None or not self.is_empty(tx, ty):
            return False
        self.set(sx, sy, None)
        self.set(tx, ty, piece)
        return True

    def occupied_count(self) -> int:
        return sum(1 for row in self.cells for cell in row if cell is not None)


@dataclass
class DragDropSystem:
    board: Board
    selected: Optional[Coord] = None

    def select(self, x: int, y: int) -> bool:
        if self.board.in_bounds(x, y) and self.board.get(x, y) is not None:
            self.selected = (x, y)
            return True
        return False

    def drop(self, x: int, y: int) -> bool:
        if self.selected is None:
            return False
        moved = self.board.move(self.selected, (x, y))
        self.selected = None
        return moved


@dataclass
class RuleEngine:
    board: Board

    def is_valid_move(self, source: Coord, target: Coord) -> bool:
        sx, sy = source
        tx, ty = target
        if not self.board.in_bounds(sx, sy) or not self.board.in_bounds(tx, ty):
            return False
        if self.board.get(sx, sy) is None:
            return False
        if not self.board.is_empty(tx, ty):
            return False
        return True

    def has_match(self, min_len: int = 3) -> bool:
        # Row scan.
        for y in range(self.board.height):
            run_piece = None
            run_len = 0
            for x in range(self.board.width):
                piece = self.board.get(x, y)
                if piece is not None and piece == run_piece:
                    run_len += 1
                else:
                    run_piece = piece
                    run_len = 1 if piece is not None else 0
                if run_len >= min_len:
                    return True
        # Column scan.
        for x in range(self.board.width):
            run_piece = None
            run_len = 0
            for y in range(self.board.height):
                piece = self.board.get(x, y)
                if piece is not None and piece == run_piece:
                    run_len += 1
                else:
                    run_piece = piece
                    run_len = 1 if piece is not None else 0
                if run_len >= min_len:
                    return True
        return False


@dataclass
class GameStateManager:
    target_matches: int
    max_moves: int
    matches_done: int = 0
    moves_used: int = 0

    def record_move(self) -> None:
        self.moves_used += 1

    def record_match(self) -> None:
        self.matches_done += 1

    def is_win(self) -> bool:
        return self.matches_done >= self.target_matches

    def is_lose(self) -> bool:
        return self.moves_used >= self.max_moves and not self.is_win()


@dataclass
class Level:
    level_id: int
    width: int
    height: int
    max_moves: int
    initial_cells: List[List[Optional[str]]]


class LevelLoader:
    @staticmethod
    def load_many(raw_json: str) -> List[Level]:
        doc = json.loads(raw_json)
        levels: List[Level] = []
        for entry in doc["levels"]:
            levels.append(
                Level(
                    level_id=entry["level_id"],
                    width=entry["width"],
                    height=entry["height"],
                    max_moves=entry["max_moves"],
                    initial_cells=entry["initial_cells"],
                )
            )
        return levels


@dataclass
class LevelFlow:
    levels: List[Level]
    current_index: int = 0

    def start(self) -> Level:
        self.current_index = 0
        return self.levels[self.current_index]

    def restart(self) -> Level:
        return self.levels[self.current_index]

    def next_level(self) -> Optional[Level]:
        if self.current_index + 1 >= len(self.levels):
            return None
        self.current_index += 1
        return self.levels[self.current_index]


@dataclass
class HUDModel:
    moves_left: int
    coin: int
    paused: bool = False

    def consume_move(self) -> None:
        self.moves_left = max(0, self.moves_left - 1)

    def toggle_pause(self) -> None:
        self.paused = not self.paused


@dataclass
class MenuManager:
    state: str = "home"

    def go_home(self) -> None:
        self.state = "home"

    def go_fail(self) -> None:
        self.state = "fail"

    def go_win(self) -> None:
        self.state = "win"


@dataclass
class FeedbackEngine:
    events: List[str] = field(default_factory=list)

    def on_move(self) -> None:
        self.events.append("move")

    def on_success(self) -> None:
        self.events.append("success")

    def on_fail(self) -> None:
        self.events.append("fail")


class ContentGenerator:
    @staticmethod
    def generate_60_levels(seed: int = 7) -> List[Level]:
        rng = random.Random(seed)
        levels: List[Level] = []
        for lid in range(1, 61):
            if lid <= 20:
                width, height, max_moves = 4, 4, 14
            elif lid <= 40:
                width, height, max_moves = 5, 5, 16
            else:
                width, height, max_moves = 6, 6, 18
            cells = [[rng.choice([None, "R", "G", "B"]) for _ in range(width)] for _ in range(height)]
            levels.append(Level(lid, width, height, max_moves, cells))
        return levels


@dataclass
class Balancer:
    def has_early_hard_block(self, levels: List[Level], first_minutes: int = 15) -> bool:
        # Simple gate: first slice should remain in easy/medium board sizes.
        check_count = min(len(levels), first_minutes)
        for level in levels[:check_count]:
            if level.width >= 6 and level.max_moves < 16:
                return True
        return False


class DailyChallengeService:
    @staticmethod
    def level_seed_for(day: date) -> int:
        digest = hashlib.sha256(day.isoformat().encode("utf-8")).hexdigest()
        return int(digest[:8], 16)


@dataclass
class StreakTracker:
    last_login: Optional[date] = None
    streak: int = 0

    def login(self, today: date) -> int:
        if self.last_login is None:
            self.streak = 1
        elif (today - self.last_login).days == 1:
            self.streak += 1
        elif (today - self.last_login).days == 0:
            pass
        else:
            self.streak = 1
        self.last_login = today
        return self.streak


@dataclass
class CoinEconomy:
    coin: int = 0

    def earn(self, amount: int) -> int:
        self.coin += max(0, amount)
        return self.coin

    def spend(self, amount: int) -> bool:
        if amount < 0 or amount > self.coin:
            return False
        self.coin -= amount
        return True


@dataclass
class BoosterService:
    board: Board
    history: List[List[List[Optional[str]]]] = field(default_factory=list)

    def snapshot(self) -> None:
        self.history.append([row[:] for row in self.board.cells])

    def undo(self) -> bool:
        if not self.history:
            return False
        self.board.cells = self.history.pop()
        return True

    def hint(self) -> Optional[Tuple[Coord, Coord]]:
        for y in range(self.board.height):
            for x in range(self.board.width):
                if self.board.get(x, y) is None:
                    continue
                for ty in range(self.board.height):
                    for tx in range(self.board.width):
                        if self.board.is_empty(tx, ty):
                            return (x, y), (tx, ty)
        return None

    def shuffle(self, seed: int = 17) -> None:
        rng = random.Random(seed)
        pieces = [cell for row in self.board.cells for cell in row if cell is not None]
        rng.shuffle(pieces)
        i = 0
        for y in range(self.board.height):
            for x in range(self.board.width):
                if self.board.get(x, y) is not None:
                    self.board.set(x, y, pieces[i])
                    i += 1


@dataclass
class AdMobService:
    rewarded_count: int = 0
    rewarded_continue_used: bool = False
    rewarded_coin_total: int = 0

    def watch_rewarded_for_continue(self) -> bool:
        self.rewarded_count += 1
        self.rewarded_continue_used = True
        return True

    def watch_rewarded_for_coin(self, coin_gain: int = 20) -> int:
        self.rewarded_count += 1
        self.rewarded_coin_total += coin_gain
        return coin_gain


@dataclass
class InterstitialPolicy:
    every_n_levels: int = 3

    def should_show(self, level_completed_count: int) -> bool:
        return level_completed_count > 0 and level_completed_count % self.every_n_levels == 0


@dataclass
class IAPStore:
    no_ads_owned: bool = False
    coin: int = 0

    def purchase(self, product_id: str) -> bool:
        if product_id == "no_ads":
            self.no_ads_owned = True
            return True
        if product_id == "coin_pack":
            self.coin += 500
            return True
        return False


@dataclass
class AnalyticsLogger:
    events: List[Tuple[str, Dict[str, str]]] = field(default_factory=list)

    def log(self, event_name: str, payload: Optional[Dict[str, str]] = None) -> None:
        self.events.append((event_name, payload or {}))


@dataclass
class AndroidBuildPipeline:
    critical_crashes: int = 0

    def release_build(self) -> bool:
        return self.critical_crashes == 0


@dataclass
class BugfixBuffer:
    p0_open: int = 0
    p1_open: int = 0

    def close_p0(self, count: int = 1) -> None:
        self.p0_open = max(0, self.p0_open - max(0, count))

    def close_p1(self, count: int = 1) -> None:
        self.p1_open = max(0, self.p1_open - max(0, count))

    def all_closed(self) -> bool:
        return self.p0_open == 0 and self.p1_open == 0


@dataclass
class StoreAssetsChecklist:
    icon: bool = False
    screenshots: int = 0
    short_video: bool = False

    def is_ready(self) -> bool:
        return self.icon and self.screenshots >= 5 and self.short_video


@dataclass
class StoreListing:
    title: str = ""
    short_description: str = ""
    long_description: str = ""
    keywords: List[str] = field(default_factory=list)

    def is_ready(self) -> bool:
        return all(
            [
                bool(self.title.strip()),
                bool(self.short_description.strip()),
                bool(self.long_description.strip()),
                len(self.keywords) >= 5,
            ]
        )
