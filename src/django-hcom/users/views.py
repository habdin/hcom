# vim: foldmethod=indent
#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from typing import Any

from django.contrib.auth.views import LoginView, LogoutView
from django.db.models import Q
from django.http import JsonResponse
from django.urls import reverse_lazy
from django.views import generic
from django.conf import settings

from .forms import UserChangeForm, UserCreateForm
from .models import User


class UserListView(generic.ListView):
    model = User
    context_object_name = "objects"
    paginate_by: int = 3

    def get_context_data(self, **kwargs):
        view_card: bool = True
        if self.request.GET.get("is_table"):
            view_card: bool = False
        context: dict[str, Any] = super().get_context_data(**kwargs)
        context["title"] = "User list"
        context["is_table"] = view_card
        context["debug"] = settings.DEBUG
        return context


class UserSearchListView(generic.ListView):
    model = User
    context_object_name = "objects"
    template_name = "users/card.html"

    def get_queryset(self, search: str | None = None):
        search = self.request.GET.get("search")
        if search:
            queryset = User.objects.filter(
                Q(username__icontains=search)
                | Q(first_name__icontains=search)
                | Q(last_name__icontains=search)
            )
        else:
            queryset = super().get_queryset()
        return queryset


class UserCreateView(generic.CreateView):
    model = User
    form_class = UserCreateForm
    success_url = reverse_lazy("users:user-list")


class UserDetailView(generic.DetailView):
    model = User


class UserUpdateView(generic.UpdateView):
    model = User
    form_class = UserChangeForm
    success_url = reverse_lazy("users:user-list")


class UserDeleteView(generic.DeleteView):
    model = User
    success_url = reverse_lazy("users:user-list")


class UserLoginView(LoginView):
    template_name = "users/user_login.html"
    success_url = reverse_lazy("site-home")


class UserLogoutView(LogoutView):
    template_name = "users/user_logout.html"


def LoadData(request):
    """Load Model data as Json into a jquery datatable and provides the server-side searching and
    filtering for the table.
    """
    if request.method == "GET":
        draw = request.GET.get("draw")
    else:
        draw = request.POST.get("draw")
    users = User.objects.all()
    count = len(users)
    ser_users = list(users.values())
    response = {
        "draw": draw,
        "data": ser_users,
        "recordsTotal": count,
        "recordsFiltered": count,
    }
    return JsonResponse(response)
