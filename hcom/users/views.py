#!/usr/bin/env python3
# -*- coding: utf-8 -*-

from django.http import JsonResponse
from django.views.generic import (CreateView, DeleteView, DetailView, ListView,
                                  UpdateView)
from rest_framework import viewsets

from .models import User
from .serializers import UserSerializer


class UserListView(ListView):
    model = User
    context_object_name = "objects"
    paginate_by = 3

    def get_context_data(self, **kwargs):
        context = super().get_context_data(**kwargs)
        context["title"] = "User list"
        return context


class UserCreateView(CreateView):
    model = User
    fields = [
        "username",
        "first_name",
        "last_name",
        "email",
        "password",
        "is_staff",
        "is_active",
        "city",
        "id_number",
        "gender",
        "marital_status",
    ]
    success_url = "/users/"


class UserUpdateView(UpdateView):
    model = User
    fields = [
        "username",
        "first_name",
        "last_name",
        "email",
        "password",
        "is_staff",
        "is_active",
        "city",
        "id_number",
        "gender",
        "marital_status",
    ]
    success_url = "/users/"

    def post(self, request, *args, **kwargs):
        super().post(request, *args, **kwargs)
        result = True
        response = {"result": result}
        return JsonResponse(response)


class UserDetailView(DetailView):
    model = User


class UserDeleteView(DeleteView):
    model = User
    success_url = "/users/"


class UserViewSet(viewsets.ModelViewSet):
    queryset = User.objects.all()
    serializer_class = UserSerializer


def LoadData(request):
    """Load Model data as Json into a jquery datatable and provides the server-side searching and
    filtering for the table.
    """
    draw = 1
    users = User.objects.all()
    count = len(users)
    ser_users = list(
        users.values(
            "id",
            "username",
            "first_name",
            "last_name",
            "email",
            "id_number",
            "is_staff",
            "gender",
            "marital_status",
            "city",
        )
    )
    response = {
        "draw": draw,
        "data": ser_users,
        "recordsTotal": count,
        "recordsFiltered": count,
    }
    return JsonResponse(response)
